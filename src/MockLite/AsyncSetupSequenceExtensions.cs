namespace MockLite;

internal static class AsyncSetupSequenceExtensions
{
    public static ISetupSequence<T, Task<TAsyncResult>> ReturnsAsync<T, TAsyncResult>(
        this ISetupSequence<T, Task<TAsyncResult>> setup, 
        TAsyncResult? value = default)
    {
        return setup.Returns(Task.FromResult(value)!);
    }
    
    public static ISetupSequence<T, Task<TAsyncResult>> ReturnsAsync<T, TAsyncResult>(
        this ISetupSequence<T, Task<TAsyncResult>> setup, 
        Func<TAsyncResult> valueFunction)
    {
        return setup.Returns(() => Task.FromResult(valueFunction()));
    }
}