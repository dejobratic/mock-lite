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
                switch (arg)
                {
                    // Try to evaluate the expression to get the actual value
                    case ConstantExpression constExpr:
                        return constExpr.Value;
                    
                    // Check if this is an It.IsAny<T>() or It.Is<T>(expression) call
                    case MethodCallExpression { Method.DeclaringType.Name: "It" } methodCall:
                    {
                        switch (methodCall.Method.Name)
                        {
                            case "IsAny":
                                return ItMarker.Any;
                            
                            case "Is" when methodCall.Arguments.Count == 1:
                            {
                                var expressionArg = methodCall.Arguments[0];
                                switch (expressionArg)
                                {
                                    case ConstantExpression { Value: LambdaExpression lambdaExpr }:
                                        return new ItMarker.ExpressionMatcher(lambdaExpr);
                                    
                                    // Handle quoted expressions
                                    case UnaryExpression { NodeType: ExpressionType.Quote, Operand: LambdaExpression quotedLambda }:
                                        return new ItMarker.ExpressionMatcher(quotedLambda);
                                }

                                break;
                            }
                        }

                        break;
                    }
                }

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