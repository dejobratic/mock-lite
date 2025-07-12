namespace MockLite.Tests.Unit.Sample.Domain;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
    public decimal DiscountPercentage { get; set; }
    public bool IsActive { get; set; }
}

public enum CustomerType
{
    Regular,
    Premium,
    Vip,
}