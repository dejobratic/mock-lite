using System.Reflection;

namespace MockLite;

internal class FuncSetup<T, TResult> : IMethodSetup, ISetup<T, TResult>
{
    private Func<TResult>? _callback;
    private Action<object[]>? _parameterCallback;
    private Exception? _exception;
    
    public object? Execute(object[] args)
    {
        try
        {
            // Execute parameter callback with arguments
            _parameterCallback?.Invoke(args);

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
    
    public void ReturnsAsync(TResult value)
        => _callback = () => value;
    
    public void ReturnsAsync(Func<TResult> valueFunction)
        => _callback = valueFunction;

    public void Throws<TException>()
        where TException : Exception, new()
        => _exception = new TException();
    
    public void Throws(Exception exception)
        => _exception = exception;
    
    public void ThrowsAsync<TException>()
        where TException : Exception, new()
        => _exception = new TException();
    
    public void ThrowsAsync(Exception exception)
        => _exception = exception;
    
    public ISetup<T, TResult> Callback(Action callback)
    {
        _parameterCallback = _ => callback();
        return this;
    }
    
    public ISetup<T, TResult> Callback(Action<object[]> callback)
    {
        _parameterCallback = callback;
        return this;
    }
}
