namespace MockLite;

internal class SequenceStep<T>
{
    public T Value { get; init; } = default!;

    public Exception Exception { get; init; } = null!;
    
    public Action<object[]>? ParameterCallback { get; set; }
}