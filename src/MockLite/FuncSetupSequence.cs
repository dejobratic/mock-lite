using System.Reflection;

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
                if (step.ParameterCallback is not null)
                    InvokeParameterCallback(step.ParameterCallback, args);
                
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
    
    private void InvokeParameterCallback(Action parameterCallback, object[] args)
    {
        try
        {
            var method = parameterCallback.Method;
            var parameters = method.GetParameters();
            
            if (parameters.Length == 0)
            {
                parameterCallback.DynamicInvoke();
            }
            else if (parameters.Length <= args.Length)
            {
                var callbackArgs = new object[parameters.Length];
                Array.Copy(args, callbackArgs, parameters.Length);
                parameterCallback.DynamicInvoke(callbackArgs);
            }
        }
        catch (TargetParameterCountException)
        {
            // Parameter count mismatch, skip callback
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
            lastStep.ParameterCallback = callback;
        }
        return this;
    }
    
    public ISetupSequence<T, TResult> Callback<T1>(Action<T1> callback)
    {
        if (_steps.Count > 0)
        {
            var lastStep = _steps.ToArray()[_steps.Count - 1];
            lastStep.ParameterCallback = args => callback((T1)args[0]);
        }
        
        return this;
    }
    
    public ISetupSequence<T, TResult> Callback<T1, T2>(Action<T1, T2> callback)
    {
        if (_steps.Count > 0)
        {
            var lastStep = _steps.ToArray()[_steps.Count - 1];
            lastStep.ParameterCallback = args => callback((T1)args[0], (T2)args[1]);
        }
        
        return this;
    }
    
    public ISetupSequence<T, TResult> Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
    {
        if (_steps.Count > 0)
        {
            var lastStep = _steps.ToArray()[_steps.Count - 1];
            lastStep.ParameterCallback = args => callback((T1)args[0], (T2)args[1], (T3)args[2]);
        }
        
        return this;
    }
    
    public ISetupSequence<T, TResult> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
    {
        if (_steps.Count > 0)
        {
            var lastStep = _steps.ToArray()[_steps.Count - 1];
            lastStep.ParameterCallback = args => callback((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
        }
        
        return this;
    }
}
