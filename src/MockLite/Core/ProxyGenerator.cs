using System.Reflection;
using System.Reflection.Emit;

namespace MockLite.Core;

public static class ProxyGenerator
{
    private static readonly Dictionary<Type, Type> ProxyTypes = new();
    
    private static readonly object Lock = new();
    
    public static T CreateProxy<T>(MockInterceptor interceptor) where T : class
    {
        var targetType = typeof(T);
        
        if (!targetType.IsInterface)
            throw new ArgumentException($"Type {targetType.Name} must be an interface", nameof(T));
        
        lock (Lock)
        {
            if (!ProxyTypes.TryGetValue(targetType, out var proxyType))
            {
                proxyType = CreateProxyType(targetType);
                ProxyTypes[targetType] = proxyType;
            }
            
            return (T)Activator.CreateInstance(proxyType, interceptor)!;
        }
    }
    
    private static Type CreateProxyType(Type interfaceType)
    {
        var assemblyName = new AssemblyName($"MockLite.Proxies.{interfaceType.Name}");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("ProxyModule");
        
        var typeBuilder = moduleBuilder.DefineType(
            $"{interfaceType.Name}Proxy",
            TypeAttributes.Public | TypeAttributes.Class,
            typeof(object),
            [interfaceType]);
        
        // Define interceptor field
        var interceptorField = typeBuilder.DefineField(
            "_interceptor", 
            typeof(MockInterceptor), 
            FieldAttributes.Private);
        
        // Define constructor
        DefineConstructor(typeBuilder, interceptorField);
        
        // Implement all interface methods (this includes property getters/setters)
        foreach (var method in interfaceType.GetMethods())
        {
            ImplementMethod(typeBuilder, method, interceptorField);
        }
        
        return typeBuilder.CreateType();
    }
    
    private static void DefineConstructor(TypeBuilder typeBuilder, FieldBuilder interceptorField)
    {
        var constructor = typeBuilder.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            [typeof(MockInterceptor)]);
        
        var il = constructor.GetILGenerator();
        
        // Call base constructor
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)!);
        
        // Store interceptor field
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Stfld, interceptorField);
        
        il.Emit(OpCodes.Ret);
    }
    
    private static void ImplementMethod(TypeBuilder typeBuilder, MethodInfo method, FieldBuilder interceptorField)
    {
        var parameters = method.GetParameters();
        var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();
        
        var methodBuilder = typeBuilder.DefineMethod(
            method.Name,
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot,
            CallingConventions.Standard,
            method.ReturnType,
            parameterTypes);
        
        // Handle generic methods
        if (method.IsGenericMethodDefinition)
        {
            var genericArguments = method.GetGenericArguments();
            var genericParameterNames = genericArguments.Select(arg => arg.Name).ToArray();
            methodBuilder.DefineGenericParameters(genericParameterNames);
        }
        
        // Define parameters with their names and attributes
        for (var i = 0; i < parameters.Length; i++)
        {
            var param = parameters[i];
            methodBuilder.DefineParameter(i + 1, param.Attributes, param.Name);
        }
        
        var il = methodBuilder.GetILGenerator();
        
        // Create an array for method arguments
        var argsLocal = il.DeclareLocal(typeof(object[]));
        il.Emit(OpCodes.Ldc_I4, parameters.Length);
        il.Emit(OpCodes.Newarr, typeof(object));
        il.Emit(OpCodes.Stloc, argsLocal);
        
        // Fill arguments array
        for (var i = 0; i < parameters.Length; i++)
        {
            il.Emit(OpCodes.Ldloc, argsLocal);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldarg, i + 1);
            
            if (parameterTypes[i].IsValueType)
                il.Emit(OpCodes.Box, parameterTypes[i]);
                
            il.Emit(OpCodes.Stelem_Ref);
        }
        
        // Call interceptor.Intercept(method, args)
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, interceptorField);
        
        // Load MethodInfo
        il.Emit(OpCodes.Ldtoken, method);
        il.Emit(OpCodes.Ldtoken, method.DeclaringType!);
        il.Emit(OpCodes.Call, typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle), [typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) ])!);
        il.Emit(OpCodes.Castclass, typeof(MethodInfo));
        
        // Load args array
        il.Emit(OpCodes.Ldloc, argsLocal);
        
        // Call Intercept
        il.Emit(OpCodes.Callvirt, typeof(MockInterceptor).GetMethod(nameof(MockInterceptor.Intercept))!);
        
        // Handle return value
        if (method.ReturnType == typeof(void))
        {
            il.Emit(OpCodes.Pop);
        }
        else if (method.ReturnType.IsValueType)
        {
            var endLabel = il.DefineLabel();
            var notNullLabel = il.DefineLabel();
            
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Brtrue_S, notNullLabel);
            il.Emit(OpCodes.Pop);
            
            // Return default value for value types
            var defaultLocal = il.DeclareLocal(method.ReturnType);
            il.Emit(OpCodes.Ldloca, defaultLocal);
            il.Emit(OpCodes.Initobj, method.ReturnType);
            il.Emit(OpCodes.Ldloc, defaultLocal);
            il.Emit(OpCodes.Br_S, endLabel);
            
            il.MarkLabel(notNullLabel);
            il.Emit(OpCodes.Unbox_Any, method.ReturnType);
            
            il.MarkLabel(endLabel);
        }
        else
        {
            il.Emit(OpCodes.Castclass, method.ReturnType);
        }
        
        il.Emit(OpCodes.Ret);
        
        typeBuilder.DefineMethodOverride(methodBuilder, method);
    }
}