namespace MockLite;

internal class ActionSetupSequence<T> : IMethodSetup, ISetupSequence<T>
{
    private readonly Queue<SequenceStep<object>> _steps = new();

    private readonly object _lock = new();

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
        _steps.Enqueue(new SequenceStep<object> { Value = default(T)! });
        return this;
    }

    public ISetupSequence<T> Throws<TException>()
        where TException : Exception, new()
    {
        _steps.Enqueue(new SequenceStep<object> { Exception = new TException() });
        return this;
    }

    public ISetupSequence<T> Throws(Exception exception)
    {
        _steps.Enqueue(new SequenceStep<object> { Exception = exception });
        return this;
    }

    public ISetupSequence<T> ReturnsAsync()
    {
        _steps.Enqueue(new SequenceStep<object> { Value = null! });
        return this;
    }

    public ISetupSequence<T> ThrowsAsync<TException>() where TException : Exception, new()
    {
        _steps.Enqueue(new SequenceStep<object> { Exception = new TException() });
        return this;
    }

    public ISetupSequence<T> ThrowsAsync(Exception exception)
    {
        _steps.Enqueue(new SequenceStep<object> { Exception = exception });
        return this;
    }

    public ISetupSequence<T> Callback(Action callback)
    {
        if (_steps.Count > 0)
        {
            var lastStep = _steps.ToArray()[_steps.Count - 1];
            lastStep.ParameterCallback = _ => callback();
        }
        
        return this;
    }

    public ISetupSequence<T> Callback(Action<object[]> callback)
    {
        if (_steps.Count > 0)
        {
            var lastStep = _steps.ToArray()[_steps.Count - 1];
            lastStep.ParameterCallback = callback;
        }
        
        return this;
    }
}