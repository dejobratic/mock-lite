using System.Reflection;

namespace MockLite;

internal class MethodCall(MethodInfo method, object[] arguments)
{
    public MethodInfo Method { get; } = method;

    public object[] Arguments { get; } = arguments;

    public bool Matches(MethodCall other)
    {
        if (Method != other.Method)
            return false;

        if (Arguments.Length != other.Arguments.Length)
            return false;

        return !Arguments.Where((t, i) => !ArgumentMatches(t, other.Arguments[i])).Any();
    }

    private static bool ArgumentMatches(object setupArg, object callArg)
        => setupArg == ArgMatcher.Any || Equals(setupArg, callArg);

    public override bool Equals(object? obj)
        => obj is MethodCall other && Matches(other);

    public override int GetHashCode()
        => Method.GetHashCode();
}