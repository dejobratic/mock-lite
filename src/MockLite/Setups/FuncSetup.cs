namespace MockLite.Setups;

internal class FuncSetup<T, TResult> : IMethodSetup, ISetup<T, TResult>
{
    private Func<TResult>? _callback;
    private Action? _simpleCallback;
    private Delegate? _parameterCallback;
    private Exception? _exception;

    public object? Execute(object[] args)
    {
        try
        {
            // Execute simple callback
            _simpleCallback?.Invoke();

            // Execute parameter callback with arguments
            _parameterCallback?.DynamicInvoke(args);

            if (_exception is not null)
                throw _exception;

            return _callback is not null
                ? _callback()
                : default;
        }
        catch (Exception ex) when (ex != _exception)
        {
            // If callback throws, let it propagate unless we have a setup exception
            throw;
        }
    }

    public void Returns(TResult value)
        => _callback = () => value;

    public void Returns(Func<TResult> valueFunction)
        => _callback = valueFunction;
    
    public void Throws<TException>()
        where TException : Exception, new()
        => _exception = new TException();

    public void Throws(Exception exception)
        => _exception = exception;
    
    public ISetup<T, TResult> Callback(Action callback)
    {
        _simpleCallback = callback;
        return this;
    }

    public ISetup<T, TResult> Callback(Delegate callback)
    {
        _parameterCallback = callback;
        return this;
    }
}