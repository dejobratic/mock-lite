using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using MockLite.Core;
using MockLite.Exceptions;
using MockLite.Setups;

namespace MockLite.Core;

public class MockInterceptor
{
    private readonly Dictionary<MethodCall, IMethodSetup> _setups = new();
    private readonly List<MethodCall> _calls = [];

    private readonly object _lock = new();

    public object? Intercept(MethodInfo method, object[] args)
    {
        var methodCall = new MethodCall(method, args);

        lock (_lock)
        {
            _calls.Add(methodCall);

            // Try to find a matching setup
            var matchingSetup = _setups.FirstOrDefault(kvp => methodCall.Matches(kvp.Key));
            if (matchingSetup.Key != null)
            {
                return matchingSetup.Value.Execute(args);
            }

            return GetDefaultValue(method.ReturnType)!; // Return default value for the return type
        }
    }

    public ISetup<T> Setup<T>(Expression<Action<T>> expression)
    {
        var methodCall = ParseExpression(expression);
        var setup = new ActionSetup<T>();
        _setups[methodCall] = setup;
        return setup;
    }

    public ISetup<T, TResult> Setup<T, TResult>(Expression<Func<T, TResult>> expression)
    {
        var methodCall = ParseExpression(expression);
        var setup = new FuncSetup<T, TResult>();
        _setups[methodCall] = setup;
        return setup;
    }

    public ISetupGetter<T, TProperty> SetupGet<T, TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var methodCall = ParseExpression(expression);
        var setup = new PropertyGetSetup<T, TProperty>();
        _setups[methodCall] = setup;
        return setup;
    }

    public ISetupSetter<T> SetupSet<T, TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var methodCall = ParseExpressionForSetter(expression);
        var setup = new PropertySetSetup<T>();
        _setups[methodCall] = setup;
        return setup;
    }
    
    private static MethodCall ParseExpressionForSetter(LambdaExpression expression)
    {
        return expression.Body switch
        {
            // Handle regular properties
            MemberExpression { Member: PropertyInfo prop } =>
                CreateSetterMethodCall(prop.GetSetMethod(), prop.Name, []),
            
            // Handle indexers (they appear as method calls to get_Item)
            MethodCallExpression methodCall when methodCall.Method.Name.StartsWith("get_") =>
                HandleGetterMethodCallForSetter(methodCall),
            
            // Handle indexers using IndexExpression (alternative syntax)
            IndexExpression indexExpr when indexExpr.Indexer != null =>
                CreateSetterMethodCall(indexExpr.Indexer.GetSetMethod(), "indexer", 
                    ExtractArgumentMatchersForSetter(indexExpr.Arguments)),
            
            _ => throw new ArgumentException("SetupSet requires a property or indexer expression")
        };
    }
    
    private static MethodCall HandleGetterMethodCallForSetter(MethodCallExpression methodCall)
    {
        // For indexers: get_Item -> set_Item
        // For properties: get_PropertyName -> set_PropertyName
        var getterName = methodCall.Method.Name;
        var setterName = getterName.Replace("get_", "set_");
        
        // Find the corresponding setter method
        var setterMethod = methodCall.Method.DeclaringType?.GetMethod(setterName);
        if (setterMethod == null)
            throw new ArgumentException($"No setter method found for '{getterName}'");
        
        // Extract arguments from the getter call
        var args = ExtractArgumentMatchersForSetter(methodCall.Arguments);
        
        return CreateSetterMethodCall(setterMethod, getterName.Replace("get_", ""), args);
    }
    
    private static MethodCall CreateSetterMethodCall(MethodInfo? setMethod, string memberName, object[] args)
    {
        if (setMethod == null)
            throw new ArgumentException($"Property '{memberName}' does not have a setter");
            
        // Add the value parameter (always use Any matcher for the value being set)
        var allArgs = args.Concat([ArgMatcher.Any]).ToArray();
        return new MethodCall(setMethod, allArgs);
    }
    
    private static object[] ExtractArgumentMatchersForSetter(ReadOnlyCollection<Expression> arguments)
    {
        return [.. arguments.Select(arg =>
        {
            try
            {
                // Handle constants and simple expressions
                switch (arg)
                {
                    case ConstantExpression constExpr:
                        return constExpr.Value;
                    
                    case MethodCallExpression { Method.DeclaringType.Name: "It" }:
                        return ArgMatcher.Any;
                }

                // For other expressions, try to evaluate them
                var lambda = Expression.Lambda(arg);
                var compiled = lambda.Compile();
                return compiled.DynamicInvoke();
            }
            catch
            {
                // If we can't evaluate the expression, use wildcard matcher
                return ArgMatcher.Any;
            }
        })!];
    }

    public void Verify<T>(Expression<Action<T>> expression, Times times)
    {
        var methodCall = ParseExpression(expression);
        var actualCalls = _calls.Count(c => c.Matches(methodCall));

        if (actualCalls < times.MinCalls || actualCalls > times.MaxCalls)
        {
            throw new MockException($"Expected {times.MinCalls}-{times.MaxCalls} calls, but got {actualCalls}");
        }
    }

    public void Verify<T, TResult>(Expression<Func<T, TResult>> expression, Times times)
    {
        var methodCall = ParseExpression(expression);
        var actualCalls = _calls.Count(c => c.Matches(methodCall));

        if (actualCalls < times.MinCalls || actualCalls > times.MaxCalls)
        {
            throw new MockException($"Expected {times.MinCalls}-{times.MaxCalls} calls, but got {actualCalls}");
        }
    }

    public ISetupSequence<T, TResult> SetupSequence<T, TResult>(Expression<Func<T, TResult>> expression)
    {
        var methodCall = ParseExpression(expression);
        var sequence = new FuncSetupSequence<T, TResult>();
        
        _setups[methodCall] = sequence;
        return sequence;
    }

    public ISetupSequence<T> SetupSequence<T>(Expression<Action<T>> expression)
    {
        var methodCall = ParseExpression(expression);
        var sequence = new ActionSetupSequence<T>();
        
        _setups[methodCall] = sequence;
        return sequence;
    }

    private static MethodCall ParseExpression(LambdaExpression expression)
        => MethodCallFactory.Create(expression);

    private static object? GetDefaultValue(Type type)
    {
        if (type == typeof(void))
            return null;

        if (type.IsValueType)
            return Activator.CreateInstance(type);

        // Handle Task and Task<T> specifically
        if (type == typeof(Task))
            return Task.CompletedTask;
            
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var resultType = type.GetGenericArguments()[0];
            var defaultResult = resultType.IsValueType ? Activator.CreateInstance(resultType) : null;
            
            // Use reflection to call Task.FromResult<T>(T) with the correct type
            var taskFromResultMethod = typeof(Task).GetMethod("FromResult")!.MakeGenericMethod(resultType);
            return taskFromResultMethod.Invoke(null, [defaultResult]);
        }

        return null;
    }
}