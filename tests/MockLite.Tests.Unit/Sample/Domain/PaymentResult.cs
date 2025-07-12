namespace MockLite.Tests.Unit.Sample.Domain;

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public DateTime ProcessedAt { get; set; }
}