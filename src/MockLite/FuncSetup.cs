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
            
            return _callback is not null ? _callback() : default(TResult);
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
    
    public Task<TResult> ReturnsAsync(TResult value)
        => Task.FromResult(value);
    
    public Task<TResult> ReturnsAsync(Func<TResult> valueFunction)
        => Task.FromResult(valueFunction());

    public void Throws<TException>()
        where TException : Exception, new()
        => _exception = new TException();
    
    public void Throws(Exception exception)
        => _exception = exception;
    
    public Task<TResult> ThrowsAsync<TException>()
        where TException : Exception, new()
        => Task.FromException<TResult>(new TException());
    
    public Task<TResult> ThrowsAsync(Exception exception)
        => Task.FromException<TResult>(exception);
    
    public ISetup<T, TResult> Callback(Action callback)
    {
        _parameterCallback = _ => callback();
        return this;
    }
    
    public ISetup<T, TResult> Callback<T1>(Action<T1> callback)
    {
        _parameterCallback = args => callback((T1)args[0]);
        return this;
    }
    
    public ISetup<T, TResult> Callback<T1, T2>(Action<T1, T2> callback)
    {
        _parameterCallback = args => callback((T1)args[0], (T2)args[1]);
        return this;
    }
    
    public ISetup<T, TResult> Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
    {
        _parameterCallback = args => callback((T1)args[0], (T2)args[1], (T3)args[2]);
        return this;
    }
    
    public ISetup<T, TResult> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
    {
        _parameterCallback = args => callback((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
        return this;
    }
}
