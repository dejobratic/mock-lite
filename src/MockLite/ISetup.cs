namespace MockLite;

public interface ISetup<T>
{
    void Returns();
    
    Task ReturnsAsync();
    
    void Throws<TException>() where TException : Exception, new();
    
    void Throws(Exception exception);
    
    Task ThrowsAsync<TException>() where TException : Exception, new();
    
    Task ThrowsAsync(Exception exception);
    
    ISetup<T> Callback(Action callback);
    
    ISetup<T> Callback<T1>(Action<T1> callback);
    
    ISetup<T> Callback<T1, T2>(Action<T1, T2> callback);
    
    ISetup<T> Callback<T1, T2, T3>(Action<T1, T2, T3> callback);
    
    ISetup<T> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback);
}

public interface ISetup<T, TResult>
{
    void Returns(TResult value);
    
    void Returns(Func<TResult> valueFunction);
    
    Task<TResult> ReturnsAsync(TResult value);
    
    Task<TResult> ReturnsAsync(Func<TResult> valueFunction);
    
    void Throws<TException>() where TException : Exception, new();
    
    void Throws(Exception exception);
    
    Task<TResult> ThrowsAsync<TException>() where TException : Exception, new();
    
    Task<TResult> ThrowsAsync(Exception exception);
    
    ISetup<T, TResult> Callback(Action callback);
    
    ISetup<T, TResult> Callback<T1>(Action<T1> callback);
    
    ISetup<T, TResult> Callback<T1, T2>(Action<T1, T2> callback);
    
    ISetup<T, TResult> Callback<T1, T2, T3>(Action<T1, T2, T3> callback);
    
    ISetup<T, TResult> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback);
}
