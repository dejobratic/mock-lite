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
            
            if (step.Exception != null)
                throw step.Exception;

            return null;
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
        throw new NotImplementedException();
    }

    public ISetupSequence<T> Callback<T1>(Action<T1> callback)
    {
        throw new NotImplementedException();
    }

    public ISetupSequence<T> Callback<T1, T2>(Action<T1, T2> callback)
    {
        throw new NotImplementedException();
    }

    public ISetupSequence<T> Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
    {
        throw new NotImplementedException();
    }

    public ISetupSequence<T> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
    {
        throw new NotImplementedException();
    }
}