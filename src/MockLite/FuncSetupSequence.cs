namespace MockLite;

internal class FuncSetupSequence<T, TResult> : IMethodSetup, ISetupSequence<T, TResult>
{
    private readonly Queue<SequenceStep<TResult?>> _steps = new();
    private readonly object _lock = new();
    
    public object? Execute(object[] args)
    {
        lock (_lock)
        {
                // No more steps, return default value
            if (_steps.Count == 0)
                return default(TResult)!;
            
            var step = _steps.Dequeue();
            
            try
            {
                // Execute callback if present
                step.ParameterCallback?.Invoke(args);

                if (step.Exception != null)
                    throw step.Exception;
                
                return step.Value;
            }
            catch (Exception ex) when (ex != step.Exception)
            {
                // If callback throws, let it propagate unless we have a setup exception
                throw;
            }
        }
    }
    
    public ISetupSequence<T, TResult> Returns(TResult value)
    {
        _steps.Enqueue(new SequenceStep<TResult?> { Value = value });
        return this;
    }
    
    public ISetupSequence<T, TResult> Returns(Func<TResult> valueFunction)
    {
        _steps.Enqueue(new SequenceStep<TResult?> { Value = valueFunction() });
        return this;
    }
    
    public ISetupSequence<T, TResult> Throws<TException>() where TException : Exception, new()
    {
        _steps.Enqueue(new SequenceStep<TResult?> { Exception = new TException() });
        return this;
    }
    
    public ISetupSequence<T, TResult> Throws(Exception exception)
    {
        _steps.Enqueue(new SequenceStep<TResult?> { Exception = exception });
        return this;
    }
    
    public ISetupSequence<T, TResult> ReturnsAsync(TResult value)
    {
        _steps.Enqueue(new SequenceStep<TResult?> { Value = value });
        return this;
    }
    
    public ISetupSequence<T, TResult> ReturnsAsync(Func<TResult> valueFunction)
    {
        _steps.Enqueue(new SequenceStep<TResult?> { Value = valueFunction() });
        return this;
    }
    
    public ISetupSequence<T, TResult> ThrowsAsync<TException>() where TException : Exception, new()
    {
        _steps.Enqueue(new SequenceStep<TResult?> { Exception = new TException() });
        return this;
    }
    
    public ISetupSequence<T, TResult> ThrowsAsync(Exception exception)
    {
        _steps.Enqueue(new SequenceStep<TResult?> { Exception = exception });
        return this;
    }
    
    public ISetupSequence<T, TResult> Callback(Action callback)
    {
        if (_steps.Count > 0)
        {
            var lastStep = _steps.ToArray()[_steps.Count - 1];
            lastStep.ParameterCallback = _ => callback();
        }
        
        return this;
    }
    
    public ISetupSequence<T, TResult> Callback(Action<object[]> callback)
    {
        if (_steps.Count > 0)
        {
            var lastStep = _steps.ToArray()[_steps.Count - 1];
            lastStep.ParameterCallback = callback;
        }
        
        return this;
    }
}
