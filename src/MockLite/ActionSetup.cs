using System.Reflection;

namespace MockLite;

internal class ActionSetup<T> : IMethodSetup, ISetup<T>
{
    private Action? _callback;
    private Action? _parameterCallback;
    private Exception? _exception;
    
    public object? Execute(object[] args)
    {
        try
        {
            // Execute parameter callback with arguments
            if (_parameterCallback is not null)
                InvokeParameterCallback(args);
            
            // Execute simple callback
            _callback?.Invoke();
            
            if (_exception != null)
                throw _exception;
            
            return null;
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
            var method = _parameterCallback.Method;
            var parameters = method.GetParameters();
            
            if (parameters.Length == 0)
            {
                _parameterCallback.DynamicInvoke();
            }
            else if (parameters.Length <= args.Length)
            {
                var callbackArgs = new object[parameters.Length];
                Array.Copy(args, callbackArgs, parameters.Length);
                _parameterCallback.DynamicInvoke(callbackArgs);
            }
        }
        catch (TargetParameterCountException)
        {
            // Parameter count mismatch, skip callback
        }
    }
    
    public void Returns() => _callback = null;
    public void Throws<TException>() where TException : Exception, new() => _exception = new TException();
    public void Throws(Exception exception) => _exception = exception;
    public Task ReturnsAsync() => Task.CompletedTask;
    public Task ThrowsAsync<TException>() where TException : Exception, new() => Task.FromException(new TException());
    public Task ThrowsAsync(Exception exception) => Task.FromException(exception);
    
    public ISetup<T> Callback(Action callback)
    {
        _callback = callback;
        return this;
    }
    
    public ISetup<T> Callback<T1>(Action<T1> callback)
    {
        _parameterCallback = args => callback((T1)args[0]);
        return this;
    }
    
    public ISetup<T> Callback<T1, T2>(Action<T1, T2> callback)
    {
        _parameterCallback = args => callback((T1)args[0], (T2)args[1]);
        return this;
    }
    
    public ISetup<T> Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
    {
        _parameterCallback = args => callback((T1)args[0], (T2)args[1], (T3)args[2]);
        return this;
    }
    
    public ISetup<T> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
    {
        _parameterCallback = args => callback((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
        return this;
    }
}
