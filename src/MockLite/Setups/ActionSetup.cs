using System.Reflection;

namespace MockLite.Setups;

internal class ActionSetup<T> : ISetup<T>, IMethodSetup
{
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

    public void ThrowsAsync<TException>()
        where TException : Exception, new()
        => _exception = new TException();

    public void ThrowsAsync(Exception exception)
        => _exception = exception;

    public ISetup<T> Callback(Action callback)
    {
        _simpleCallback = callback;
        return this;
    }

    public ISetup<T> Callback(Delegate callback)
    {
        _parameterCallback = callback;
        return this;
    }
}