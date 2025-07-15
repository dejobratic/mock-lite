namespace MockLite.Tests.Unit;

public class AsyncMethodsWithReturnValuesTests
{
    private interface IAsyncMethodsWithReturnValues
    {
        Task<decimal> CalculateOrderTotalAsync(int orderId, string customerEmail);
    
        Task<bool> ProcessOrderAsync(int orderId, string customerEmail, decimal amount, string paymentMethod);
    
        Task<string> GetOrderStatusAsync(int orderId, string customerEmail, bool includeTracking);
    
        Task<bool> ValidateOrderAsync(int orderId, string customerEmail, string shippingAddress, string billingAddress);
    }

    private readonly Mock<IAsyncMethodsWithReturnValues> _sut = new();
}