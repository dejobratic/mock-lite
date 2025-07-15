namespace MockLite.Tests.Unit.Samples;

public interface ISyncVoidMethods
{
    void LogOrderCreation(string customerEmail);
    
    void LogOrderProcessing(string customerEmail, int orderId);
    
    void LogOrderCompletion(string customerEmail, int orderId, decimal total);
    
    void LogShippingUpdate(string customerEmail, int orderId, string trackingNumber, string carrier);
}