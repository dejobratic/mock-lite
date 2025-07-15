namespace MockLite.Tests.Unit.Samples;

public interface IAsyncVoidMethods
{
    Task SendOrderConfirmationAsync(int orderId, string customerEmail);
    
    Task LogOrderProcessingAsync(string customerEmail, int orderId, decimal amount);
    
    Task UpdateInventoryAsync(int orderId, string customerEmail, string warehouseId);
    
    Task ScheduleFulfillmentAsync(int orderId, string customerEmail, string shippingAddress, DateTime requestedDate);
}