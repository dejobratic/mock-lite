using MockLite.Tests.Unit.Sample.Domain;

namespace MockLite.Tests.Unit.Sample.Builders;

public class CustomerBuilder
{
    private int _id = 1;
    private string _name = "John Doe";
    private string _email = "john.doe@example.com";
    private CustomerType _type = CustomerType.Regular;
    private decimal _discountPercentage = 0m;
    private bool _isActive = true;

    public static CustomerBuilder Create() => new();

    public static Customer CreateValid() => new CustomerBuilder().Build();

    public static Customer CreatePremium() => new CustomerBuilder().WithType(CustomerType.Premium).Build();

    public static Customer CreateVIP() => new CustomerBuilder().WithType(CustomerType.Vip).Build();

    public static Customer CreateInactive() => new CustomerBuilder().WithIsActive(false).Build();

    public CustomerBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CustomerBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CustomerBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public CustomerBuilder WithType(CustomerType type)
    {
        _type = type;
        return this;
    }

    public CustomerBuilder WithDiscountPercentage(decimal percentage)
    {
        _discountPercentage = percentage;
        return this;
    }

    public CustomerBuilder WithIsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }

    public Customer Build() => new()
    {
        Id = _id,
        Name = _name,
        Email = _email,
        Type = _type,
        DiscountPercentage = _discountPercentage,
        IsActive = _isActive
    };
}