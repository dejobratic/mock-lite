namespace MockLite;

public interface ISetupSequence<T>
{
    ISetupSequence<T> Returns();
    
    ISetupSequence<T> ReturnsAsync();
    
    ISetupSequence<T> Throws<TException>()
        where TException : Exception, new();
    
    ISetupSequence<T> Throws(Exception exception);
    
    ISetupSequence<T> ThrowsAsync(Exception exception);
    
    ISetupSequence<T> ThrowsAsync<TException>()
        where TException : Exception, new();  
    
    ISetupSequence<T> Callback(Action callback);
    
    ISetupSequence<T> Callback(Action<object[]> callback);
}

public interface ISetupSequence<T, in TResult>
{
    ISetupSequence<T, TResult> Returns(TResult value);
    
    ISetupSequence<T, TResult> Returns(Func<TResult> valueFunction);
    
    ISetupSequence<T, TResult> ReturnsAsync(TResult value);
    
    ISetupSequence<T, TResult> ReturnsAsync(Func<TResult> valueFunction);
    
    ISetupSequence<T, TResult> Throws<TException>()
        where TException : Exception, new();
    
    ISetupSequence<T, TResult> Throws(Exception exception);
    
    ISetupSequence<T, TResult> ThrowsAsync<TException>()
        where TException : Exception, new();
    
    ISetupSequence<T, TResult> ThrowsAsync(Exception exception);
    
    ISetupSequence<T, TResult> Callback(Action callback);
    
    ISetupSequence<T, TResult> Callback(Action<object[]> callback);
}
