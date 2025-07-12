using MockLite.Tests.Unit.Sample.Domain;

namespace MockLite.Tests.Unit.Sample.Builders;

public class ProductBuilder
{
    private int _id = 1;
    private string _name = "Test Product";
    private decimal _price = 10.99m;
    private int _stockQuantity = 100;
    private string _category = "Electronics";
    private bool _isActive = true;

    public static ProductBuilder Create() => new();

    public static Product CreateValid() => new ProductBuilder().Build();

    public static Product CreateLowStock() => new ProductBuilder().WithStockQuantity(3).Build();

    public static Product CreateOutOfStock() => new ProductBuilder().WithStockQuantity(0).Build();

    public static Product CreateInactive() => new ProductBuilder().WithIsActive(false).Build();

    public static List<Product> CreateValidList(int count = 3)
    {
        var products = new List<Product>();
        for (int i = 1; i <= count; i++)
        {
            products.Add(new ProductBuilder()
                .WithId(i)
                .WithName($"Product {i}")
                .WithPrice(10m + i)
                .Build());
        }
        return products;
    }

    public ProductBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductBuilder WithStockQuantity(int quantity)
    {
        _stockQuantity = quantity;
        return this;
    }

    public ProductBuilder WithCategory(string category)
    {
        _category = category;
        return this;
    }

    public ProductBuilder WithIsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }

    public Product Build() => new()
    {
        Id = _id,
        Name = _name,
        Price = _price,
        StockQuantity = _stockQuantity,
        Category = _category,
        IsActive = _isActive
    };
}