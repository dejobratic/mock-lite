namespace MockLite.Tests.Unit.Samples;

public interface IPropertyTestInterface
{
    string CurrentCustomerEmail { get; set; }
    
    int MaxProcessingTimeoutSeconds { get; set; }
    
    bool IsValidationEnabled { get; set; }
    
    string DefaultShippingMethod { get; set; }
    
    decimal CurrentTaxRate { get; }
    
    bool IsServiceOnline { get; }
}