using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace MockLite;

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

    public ISetupSetter<T> SetupSet<T, TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var methodCall = ParseExpressionForSetter(expression);
        var setup = new PropertySetSetup<T>();
        _setups[methodCall] = setup;
        return setup;
    }
    
    private static MethodCall ParseExpressionForSetter(LambdaExpression expression)
    {
        if (expression.Body is MemberExpression { Member: PropertyInfo prop })
        {
            var setMethod = prop.GetSetMethod();
            if (setMethod == null)
                throw new ArgumentException($"Property '{prop.Name}' does not have a setter");
            // Property setters accept one parameter (the value), use wildcard matching
            return new MethodCall(setMethod, [ArgMatcher.Any]);
        }
        
        throw new ArgumentException("SetupSet requires a property expression");
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