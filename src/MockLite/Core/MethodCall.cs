using System.Reflection;

namespace MockLite.Core;

internal class MethodCall(MethodInfo method, object[] arguments)
{
    private MethodInfo Method { get; } = method;

    private object[] Arguments { get; } = arguments;

    public bool Matches(MethodCall other)
    {
        if (!MethodsMatch(Method, other.Method))
            return false;

        if (Arguments.Length != other.Arguments.Length)
            return false;

        return !Arguments.Where((t, i) => !ArgumentMatches(t, other.Arguments[i])).Any();
    }
    
    private static bool MethodsMatch(MethodInfo method1, MethodInfo method2)
    {
        // First try direct comparison
        if (method1 == method2)
            return true;
            
        // Performance optimization: check basic properties first
        if (method1.Name != method2.Name || method1.DeclaringType != method2.DeclaringType)
            return false;
            
        // For generic methods, we need special handling
        if (method1.IsGenericMethod || method2.IsGenericMethod)
        {
            // Get the generic method definitions for comparison
            var def1 = method1.IsGenericMethod ? 
                (method1.IsGenericMethodDefinition ? method1 : method1.GetGenericMethodDefinition()) : method1;
            var def2 = method2.IsGenericMethod ? 
                (method2.IsGenericMethodDefinition ? method2 : method2.GetGenericMethodDefinition()) : method2;
            
            // If the definitions don't match, they're different methods
            if (def1 != def2)
                return false;
            
            // If both are generic, we need to consider them compatible
            // This allows matching between Process<string> and Process<T>
            if (method1.IsGenericMethod && method2.IsGenericMethod)
            {
                // They match if they have the same generic method definition
                // We'll use argument matching to handle the specific type constraints
                return true;
            }
            
            // If one is generic and one isn't, they don't match
            return false;
        }
        
        // For non-generic methods, check parameter types and return type
        if (method1.ReturnType != method2.ReturnType)
            return false;
            
        var params1 = method1.GetParameters();
        var params2 = method2.GetParameters();
        
        if (params1.Length != params2.Length)
            return false;
            
        return params1.Select(p => p.ParameterType).SequenceEqual(params2.Select(p => p.ParameterType));
    }

    private static bool ArgumentMatches(object setupArg, object callArg)
    {
        if (setupArg == ArgMatcher.Any || setupArg == ItMarker.Any)
            return true;
            
        if (setupArg is ItMarker.ExpressionMatcher expressionMatcher)
            return expressionMatcher.Matches(callArg);
            
        if (callArg == ArgMatcher.Any || callArg == ItMarker.Any)
            return true;
            
        if (callArg is ItMarker.ExpressionMatcher callExpressionMatcher)
            return callExpressionMatcher.Matches(setupArg);
            
        return Equals(setupArg, callArg);
    }

    public override bool Equals(object? obj)
        => obj is MethodCall other && Matches(other);

    public override int GetHashCode()
    {
        // Use a combination of method name and declaring type for consistent hashing
        // This works better with our improved method matching logic
        var hash = HashCode.Combine(Method.Name, Method.DeclaringType);
        
        // For generic methods, include the definition to ensure proper grouping
        if (Method.IsGenericMethod)
        {
            var def = Method.IsGenericMethodDefinition ? Method : Method.GetGenericMethodDefinition();
            hash = HashCode.Combine(hash, def.GetHashCode());
        }
        else
        {
            hash = HashCode.Combine(hash, Method.GetHashCode());
        }
        
        return hash;
    }
}