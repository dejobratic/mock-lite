using System.Reflection;

namespace MockLite.Setups;

internal class ActionSetupSequence<T> : IMethodSetup, ISetupSequence<T>
{
    private readonly Queue<SequenceStep<object>> _steps = new();
    private readonly object _lock = new();
    private Action<object[]>? _pendingCallback;

    public object? Execute(object[] args)
    {
        lock (_lock)
        {
            // No more steps, do nothing
            if (_steps.Count == 0)
                return null;

            var step = _steps.Dequeue();

            try
            {
                // Execute callback if present
                step.ParameterCallback?.Invoke(args);

                switch (step.Value)
                {
                    // Execute step callback if present
                    case Action action:
                        action.Invoke();
                        break;
                    case Delegate del:
                        del.DynamicInvoke();
                        break;
                }

                if (step.Exception != null)
                    throw step.Exception;

                return null;
            }
            catch (Exception ex) when (ex != step.Exception)
            {
                // If callback throws, let it propagate unless we have a setup exception
                throw;
            }
        }
    }

    public ISetupSequence<T> Returns()
    {
        var step = new SequenceStep<object> { Value = default(T)! };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }

        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T> Throws<TException>()
        where TException : Exception, new()
    {
        var step = new SequenceStep<object> { Exception = new TException() };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }

        _steps.Enqueue(step);
        return this;
    }

    public ISetupSequence<T> Throws(Exception exception)
    {
        var step = new SequenceStep<object> { Exception = exception };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }

        _steps.Enqueue(step);
        return this;
    }

    public ISetupSequence<T> Callback(Action callback)
    {
        var step = new SequenceStep<object> { Value = callback };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        
        _steps.Enqueue(step);
        return this;
    }

    public ISetupSequence<T> Callback(Delegate callback)
    {
        var step = new SequenceStep<object> { Value = callback };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        
        _steps.Enqueue(step);
        return this;
    }
}