using System.Linq.Expressions;
using MockLite.Core;

namespace MockLite;

public static class It
{
    public static T IsAny<T>()
    {
        return (T)ItMarker.Any;
    }

    public static T Is<T>(Expression<Func<T, bool>> match)
    {
        return (T)(object)new ItMarker.ExpressionMatcher(match);
    }
}