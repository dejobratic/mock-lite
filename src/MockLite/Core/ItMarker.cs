using System.Linq.Expressions;

namespace MockLite.Core;

internal static class ItMarker
{
    public static readonly object Any = new();

    internal class ExpressionMatcher(LambdaExpression expression)
    {
        public bool Matches(object value)
        {
            try
            {
                var compiledExpression = expression.Compile();
                return (bool)compiledExpression.DynamicInvoke(value)!;
            }
            catch
            {
                return false;
            }
        }
    }
}