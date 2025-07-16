namespace MockLite;

public interface ISetupSequence<T>
{
    ISetupSequence<T> Throws<TException>()
        where TException : Exception, new();
    
    ISetupSequence<T> Throws(Exception exception);
    
    ISetupSequence<T> Callback(Action callback);
    
    ISetupSequence<T> Callback(Delegate callback);
}

public interface ISetupSequence<T, in TResult>
{
    ISetupSequence<T, TResult> Returns(TResult value);
    
    ISetupSequence<T, TResult> Returns(Func<TResult> valueFunction);
    
    ISetupSequence<T, TResult> Throws<TException>()
        where TException : Exception, new();
    
    ISetupSequence<T, TResult> Throws(Exception exception);
    
    ISetupSequence<T, TResult> Callback(Action callback);
    
    ISetupSequence<T, TResult> Callback(Delegate callback);
}
