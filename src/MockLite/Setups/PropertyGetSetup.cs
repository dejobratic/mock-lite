using System.Linq.Expressions;

namespace MockLite.Setups;

internal class PropertyGetSetup<T, TProperty> : IMethodSetup, ISetupGetter<T, TProperty>
{
    private Func<TProperty>? _callback;
    private Action? _parameterCallback;
    private Exception? _exception;

    public object? Execute(object[] args)
    {
        try
        {
            // Execute parameter callback
            _parameterCallback?.Invoke();

            if (_exception != null)
                throw _exception;

            return _callback is not null ? _callback() : default;
        }
        catch (Exception ex) when (ex != _exception)
        {
            // If callback throws, let it propagate unless we have a setup exception
            throw;
        }
    }

    public void Returns(TProperty value)
        => _callback = () => value;

    public void Returns(Func<TProperty> valueFunction)
        => _callback = valueFunction;

    public void Throws<TException>()
        where TException : Exception, new()
        => _exception = new TException();

    public void Throws(Exception exception)
        => _exception = exception;

    public ISetupGetter<T, TProperty> Callback(Action callback)
    {
        _parameterCallback = callback;
        return this;
    }
}