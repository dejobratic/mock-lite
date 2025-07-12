using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace MockLite;

internal class MockInterceptor
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

            return _setups.TryGetValue(methodCall, out var setup)
                ? setup.Execute(args)
                : GetDefaultValue(method.ReturnType)!; // Return default value for the return type
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

    private MethodCall ParseExpression(LambdaExpression expression)
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

    private object[] ExtractArgumentMatchers(ReadOnlyCollection<Expression> arguments)
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

    private static object? GetDefaultValue(Type type)
    {
        if (type == typeof(void))
            return null;

        if (type.IsValueType)
            return Activator.CreateInstance(type);

        return null;
    }
}