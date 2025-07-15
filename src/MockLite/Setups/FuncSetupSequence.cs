namespace MockLite.Setups;

internal class FuncSetupSequence<T, TResult>: IMethodSetup, ISetupSequence<T, TResult>
{
    private readonly Queue<SequenceStep<TResult?>> _steps = new();
    private readonly object _lock = new();
    private Action<object[]>? _pendingCallback;
    
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
        var step = new SequenceStep<TResult?> { Value = value };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T, TResult> Returns(Func<TResult> valueFunction)
    {
        var step = new SequenceStep<TResult?> { Value = valueFunction() };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T, TResult> Throws<TException>() where TException : Exception, new()
    {
        var step = new SequenceStep<TResult?> { Exception = new TException() };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T, TResult> Throws(Exception exception)
    {
        var step = new SequenceStep<TResult?> { Exception = exception };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T, TResult> ReturnsAsync(TResult value)
    {
        var step = new SequenceStep<TResult?> { Value = value };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T, TResult> ReturnsAsync(Func<TResult> valueFunction)
    {
        var step = new SequenceStep<TResult?> { Value = valueFunction() };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T, TResult> ThrowsAsync<TException>() where TException : Exception, new()
    {
        var step = new SequenceStep<TResult?> { Exception = new TException() };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T, TResult> ThrowsAsync(Exception exception)
    {
        var step = new SequenceStep<TResult?> { Exception = exception };
        if (_pendingCallback != null)
        {
            step.ParameterCallback = _pendingCallback;
            _pendingCallback = null;
        }
        _steps.Enqueue(step);
        return this;
    }
    
    public ISetupSequence<T, TResult> Callback(Action callback)
    {
        _pendingCallback = _ => callback();
        return this;
    }
    
    public ISetupSequence<T, TResult> Callback(Delegate callback)
    {
        _pendingCallback = args => callback.DynamicInvoke(args);
        return this;
    }
}
