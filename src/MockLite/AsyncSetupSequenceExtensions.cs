namespace MockLite;

public static class AsyncSetupSequenceExtensions
{
    public static ISetupSequence<T, Task<TAsyncResult>> ReturnsAsync<T, TAsyncResult>(
        this ISetupSequence<T, Task<TAsyncResult>> setup, 
        TAsyncResult value)
    {
        return setup.Returns(Task.FromResult(value));
    }
    
    public static ISetupSequence<T, Task<TAsyncResult>> ReturnsAsync<T, TAsyncResult>(
        this ISetupSequence<T, Task<TAsyncResult>> setup, 
        Func<TAsyncResult> valueFunction)
    {
        return setup.Returns(() => Task.FromResult(valueFunction()));
    }
    
    public static ISetupSequence<T, Task<TAsyncResult>> ThrowsAsync<T, TAsyncResult, TException>(
        this ISetupSequence<T, Task<TAsyncResult>> setup)
        where TException : Exception, new()
    {
        return setup.Returns(Task.FromException<TAsyncResult>(new TException()));
    }
    
    public static ISetupSequence<T, Task<TAsyncResult>> ThrowsAsync<T, TAsyncResult>(
        this ISetupSequence<T, Task<TAsyncResult>> setup, 
        Exception exception)
    {
        return setup.Returns(Task.FromException<TAsyncResult>(exception));
    }
}