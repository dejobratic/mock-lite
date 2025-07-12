using System.Reflection;

namespace MockLite;

internal class FuncSetup<T, TResult> : IMethodSetup, ISetup<T, TResult>
{
    private Func<TResult>? _callback;
    private Action? _parameterCallback;
    private Exception? _exception;
    
    public object? Execute(object[] args)
    {
        try
        {
            // Execute parameter callback with arguments
            if (_parameterCallback is not null)
                InvokeParameterCallback(args);
            
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
    
    private void InvokeParameterCallback(object[] args)
    {
        try
        {
            // Use reflection to invoke the callback with the right number of parameters
            var method = _parameterCallback?.Method;
            var parameters = method?.GetParameters() ?? [];
            
            if (parameters.Length == 0)
            {
                _parameterCallback?.DynamicInvoke();
            }
            else if (parameters.Length <= args.Length)
            {
                var callbackArgs = new object[parameters.Length];
                Array.Copy(args, callbackArgs, parameters.Length);
                _parameterCallback?.DynamicInvoke(callbackArgs);
            }
        }
        catch (TargetParameterCountException)
        {
            // Parameter count mismatch, skip callback
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
        _parameterCallback = callback;
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
