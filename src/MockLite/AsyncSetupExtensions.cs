namespace MockLite;

internal static class AsyncSetupExtensions
{
    public static void ReturnsAsync<T, TAsyncResult>(
        this ISetup<T, Task<TAsyncResult>> setup,
        TAsyncResult? value = default)
    {
        setup.Returns(Task.FromResult(value)!);
    }

    public static void ReturnsAsync<T, TAsyncResult>(
        this ISetup<T, Task<TAsyncResult>> setup,
        Func<TAsyncResult> valueFunction)
    {
        setup.Returns(() => Task.FromResult(valueFunction()));
    }
}