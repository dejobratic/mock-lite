using System.Linq.Expressions;

namespace MockLite;

public interface ISetup<T>
{
    void Throws<TException>()
        where TException : Exception, new();
    
    void Throws(Exception exception);
    
    void ThrowsAsync<TException>()
        where TException : Exception, new();
    
    void ThrowsAsync(Exception exception);
    
    ISetup<T> Callback(Action callback);
    
    ISetup<T> Callback(Delegate callback);
}

public interface ISetup<T, in TResult>
{
    void Returns(TResult value);
    
    void Returns(Func<TResult> valueFunction);
    
    void ReturnsAsync(TResult value);
    
    void ReturnsAsync(Func<TResult> valueFunction);
    
    void Throws<TException>()
        where TException : Exception, new();
    
    void Throws(Exception exception);
    
    void ThrowsAsync<TException>()
        where TException : Exception, new();
    
    void ThrowsAsync(Exception exception);
    
    ISetup<T, TResult> Callback(Action callback);
    
    ISetup<T, TResult> Callback(Delegate callback);
}

