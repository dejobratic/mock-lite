using MockLite.Tests.Unit.Sample.Domain;

namespace MockLite.Tests.Unit.Sample.Builders;

public class OrderItemBuilder
{
    private int _productId = 1;
    private Product _product = ProductBuilder.CreateValid();
    private int _quantity = 2;
    private decimal _unitPrice = 10.99m;

    public static OrderItemBuilder Create() => new();

    public static OrderItem CreateValid() => new OrderItemBuilder().Build();

    public static List<OrderItem> CreateValidList(int count = 2)
    {
        var items = new List<OrderItem>();
        for (int i = 1; i <= count; i++)
        {
            items.Add(new OrderItemBuilder()
                .WithProductId(i)
                .WithProduct(ProductBuilder.Create().WithId(i).WithName($"Product {i}").Build())
                .WithQuantity(i)
                .WithUnitPrice(10m + i)
                .Build());
        }
        return items;
    }

    public OrderItemBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public OrderItemBuilder WithProduct(Product product)
    {
        _product = product;
        return this;
    }

    public OrderItemBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public OrderItemBuilder WithUnitPrice(decimal unitPrice)
    {
        _unitPrice = unitPrice;
        return this;
    }

    public OrderItem Build() => new()
    {
        ProductId = _productId,
        Product = _product,
        Quantity = _quantity,
        UnitPrice = _unitPrice
    };
}