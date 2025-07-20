using System.Reflection;

namespace MockLite.Core;

internal class MethodCall(MethodInfo method, object[] arguments)
{
    private MethodInfo Method { get; } = method;

    private object[] Arguments { get; } = arguments;

    public bool Matches(MethodCall other)
    {
        if (Method != other.Method)
            return false;

        if (Arguments.Length != other.Arguments.Length)
            return false;

        return !Arguments.Where((t, i) => !ArgumentMatches(t, other.Arguments[i])).Any();
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
        => Method.GetHashCode();
}