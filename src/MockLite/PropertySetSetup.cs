namespace MockLite;

internal class PropertySetSetup<T> : IMethodSetup, ISetupSetter<T>
{
    private Action? _callback;
    private Action<object[]>? _parameterCallback;
    private Exception? _exception;
    
    public object? Execute(object[] args)
    {
        try
        {
            // Execute parameter callback with arguments
            if (_parameterCallback is not null)
                _parameterCallback(args);
            
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
    
    public void Throws<TException>()
        where TException : Exception, new()
        => _exception = new TException();
    
    public void Throws(Exception exception)
        => _exception = exception;
    
    public ISetupSetter<T> Callback(Action callback)
    {
        _callback = callback;
        return this;
    }
    
    public ISetupSetter<T> Callback(Action<object[]> callback)
    {
        _parameterCallback = callback;
        return this;
    }
}