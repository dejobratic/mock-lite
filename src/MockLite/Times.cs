namespace MockLite;

public class Times
{
    public int MinCalls { get; }
    public int MaxCalls { get; }
    
    private Times(int minCalls, int maxCalls)
    {
        MinCalls = minCalls;
        MaxCalls = maxCalls;
    }
    
    public static readonly Times Never = new Times(0, 0);
    
    public static readonly Times Once = new Times(1, 1);
    
    public static readonly Times AtLeastOnce = new Times(1, int.MaxValue);
    
    public static readonly Times AtMostOnce = new Times(0, 1);
    
    public static Times Exactly(int count)
        => new Times(count, count);
    
    public static Times AtLeast(int count)
        => new Times(count, int.MaxValue);
    
    public static Times AtMost(int count)
        => new Times(0, count);
    
    public static Times Between(int min, int max)
        => new Times(min, max);
}