namespace MockLite;

public static class AsyncSetupExtensions
{
    public static void ReturnsAsync<T, TAsyncResult>(this ISetup<T, Task<TAsyncResult>> setup, TAsyncResult value)
    {
        setup.Returns(Task.FromResult(value));
    }
    
    public static void ReturnsAsync<T, TAsyncResult>(this ISetup<T, Task<TAsyncResult>> setup, Func<TAsyncResult> valueFunction)
    {
        setup.Returns(() => Task.FromResult(valueFunction()));
    }
    
}