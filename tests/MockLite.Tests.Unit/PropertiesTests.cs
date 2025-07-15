namespace MockLite.Tests.Unit;

public class PropertiesTests
{
    private interface IProperties
    {
        string CurrentCustomerEmail { get; set; }
    
        int MaxProcessingTimeoutSeconds { get; set; }
    
        bool IsValidationEnabled { get; set; }
    
        string DefaultShippingMethod { get; set; }
    
        decimal CurrentTaxRate { get; }
    
        bool IsServiceOnline { get; }
    }
    
    private readonly Mock<IProperties> _sut = new();
}