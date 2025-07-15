using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace MockLite.Core;

internal static class MethodCallFactory
{
    public static MethodCall Create(LambdaExpression expression)
    {
        // Parse the expression tree to extract method info and arguments
        // This is a simplified version - full implementation would handle more cases
        return expression.Body switch
        {
            MethodCallExpression methodCall
                => new MethodCall(methodCall.Method, ExtractArgumentMatchers(methodCall.Arguments)),

            MemberExpression { Member: PropertyInfo prop }
                => new MethodCall(prop.GetGetMethod()!, []),

            _ => throw new ArgumentException("Unsupported expression type")
        };
    }
    
    private static object[] ExtractArgumentMatchers(ReadOnlyCollection<Expression> arguments)
    {
        // For now, just extract constant values
        // Full implementation would handle It.IsAny<T>(), etc.
        return [.. arguments.Select(arg =>
        {
            if (arg is ConstantExpression constExpr)
                return constExpr.Value;
            
            return ArgMatcher.Any; // Placeholder for argument matchers
        })!];
    }
}