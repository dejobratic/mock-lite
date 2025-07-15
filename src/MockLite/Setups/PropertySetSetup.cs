using System.Linq.Expressions;

namespace MockLite.Setups;

internal class PropertySetSetup<T> : IMethodSetup, ISetupSetter<T>
{
    private Action? _callback;
    private Delegate? _parameterCallback;
    private Exception? _exception;

    public object? Execute(object[] args)
    {
        try
        {
            // Execute simple callback
            _callback?.Invoke();
            
            // Execute parameter callback with arguments
            _parameterCallback?.DynamicInvoke(args);

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

    public ISetupSetter<T> Callback(Delegate callback)
    {
        _parameterCallback = callback;
        return this;
    }
}