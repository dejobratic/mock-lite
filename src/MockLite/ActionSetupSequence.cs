namespace MockLite;

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
                if (step.ParameterCallback is not null)
                    step.ParameterCallback(args);
                
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

    public ISetupSequence<T> ReturnsAsync()
    {
        var step = new SequenceStep<object> { Value = null! };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }

    public ISetupSequence<T> ThrowsAsync<TException>() where TException : Exception, new()
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

    public ISetupSequence<T> ThrowsAsync(Exception exception)
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
        _pendingCallback = _ => callback();
        return this;
    }

    public ISetupSequence<T> Callback(Action<object[]> callback)
    {
        _pendingCallback = callback;
        return this;
    }
    
    public ISetupSequence<T> Callback<T1>(Action<T1> callback)
    {
        _pendingCallback = args => callback((T1)args[0]);
        return this;
    }
    
    public ISetupSequence<T> Callback<T1, T2>(Action<T1, T2> callback)
    {
        _pendingCallback = args => callback((T1)args[0], (T2)args[1]);
        return this;
    }
    
    public ISetupSequence<T> Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
    {
        _pendingCallback = args => callback((T1)args[0], (T2)args[1], (T3)args[2]);
        return this;
    }
    
    public ISetupSequence<T> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
    {
        _pendingCallback = args => callback((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
        return this;
    }
}