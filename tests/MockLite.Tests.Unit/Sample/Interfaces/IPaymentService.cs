using MockLite.Tests.Unit.Sample.Domain;

namespace MockLite.Tests.Unit.Sample.Interfaces;

public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(decimal amount, string paymentMethod);
    Task<PaymentResult> ProcessRefundAsync(string transactionId, decimal amount);
    Task<bool> ValidatePaymentMethodAsync(string paymentMethod);
    Task SendPaymentNotificationAsync(string email, PaymentResult result);
    bool IsPaymentMethodSupported(string paymentMethod);
    decimal GetProcessingFee(decimal amount);
}