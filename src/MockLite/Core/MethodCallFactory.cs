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
        return [.. arguments.Select(arg =>
        {
            try
            {
                // Try to evaluate the expression to get the actual value
                if (arg is ConstantExpression constExpr)
                    return constExpr.Value;
                
                // For member access expressions (like variables), compile and execute
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
}