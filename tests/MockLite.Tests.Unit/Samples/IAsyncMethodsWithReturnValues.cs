namespace MockLite.Tests.Unit.Samples;

public interface IAsyncMethodsWithReturnValues
{
    Task<decimal> CalculateOrderTotalAsync(int orderId, string customerEmail);
    
    Task<bool> ProcessOrderAsync(int orderId, string customerEmail, decimal amount, string paymentMethod);
    
    Task<string> GetOrderStatusAsync(int orderId, string customerEmail, bool includeTracking);
    
    Task<bool> ValidateOrderAsync(int orderId, string customerEmail, string shippingAddress, string billingAddress);
}