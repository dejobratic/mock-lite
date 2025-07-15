namespace MockLite.Tests.Unit.Samples;

public interface ISyncMethodsWithReturnValues
{
    bool ValidateOrder(int orderId);
    
    decimal CalculateShipping(decimal weight, string destination);
    
    decimal CalculateTax(int orderId, string customerState, decimal subtotal);
    
    string GenerateOrderConfirmation(int orderId, string customerEmail, decimal total, string paymentMethod);
}