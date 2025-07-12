using MockLite.Tests.Unit.Sample.Domain;

namespace MockLite.Tests.Unit.Sample.Interfaces;

public interface INotificationService
{
    Task SendOrderConfirmationAsync(string email, Order order);
    Task SendShippingNotificationAsync(string email, int orderId, string trackingNumber);
    Task SendLowStockAlertAsync(Product product);
    void LogNotification(string message);
    bool IsEmailValid(string email);
}