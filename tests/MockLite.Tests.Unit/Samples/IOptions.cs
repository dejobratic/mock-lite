namespace MockLite.Tests.Unit.Samples;

public interface IOptions
{
    string ConnectionString { get; set; }
    
    int MaxRetries { get; set; }
    
    bool IsEnabled { get; set; }
    
    decimal Timeout { get; }  // Read-only property
}