namespace MockLite.Tests.Unit;

public class SyncVoidMethodsTests
{
    private interface ISyncVoidMethods
    {
        void LogOrderCreation(string customerEmail);
    
        void LogOrderProcessing(string customerEmail, int orderId);
    
        void LogOrderCompletion(string customerEmail, int orderId, decimal total);
    
        void LogShippingUpdate(string customerEmail, int orderId, string trackingNumber, string carrier);
    }
    
    private readonly Mock<ISyncVoidMethods> _sut = new();
}