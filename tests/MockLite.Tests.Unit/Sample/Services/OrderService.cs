using MockLite.Tests.Unit.Sample.Domain;
using MockLite.Tests.Unit.Sample.Interfaces;

namespace MockLite.Tests.Unit.Sample.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IPaymentService paymentService,
        INotificationService notificationService)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _paymentService = paymentService;
        _notificationService = notificationService;
    }

    public async Task<Order> CreateOrderAsync(int customerId, List<OrderItem> items, string paymentMethod)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("Order must contain at least one item", nameof(items));

        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            throw new InvalidOperationException($"Customer with ID {customerId} not found");

        if (!customer.IsActive)
            throw new InvalidOperationException("Cannot create order for inactive customer");

        // Validate products and stock
        foreach (var item in items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {item.ProductId} not found");

            if (!product.IsActive)
                throw new InvalidOperationException($"Product {product.Name} is not active");

            if (product.StockQuantity < item.Quantity)
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}");

            item.Product = product;
            item.UnitPrice = product.Price;
        }

        var order = new Order
        {
            CustomerId = customerId,
            Customer = customer,
            Items = items,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };

        // Calculate totals
        order.TotalAmount = items.Sum(i => i.TotalPrice);
        order.DiscountAmount = CalculateDiscount(customer, order.TotalAmount);
        order.TotalAmount -= order.DiscountAmount;

        // Process payment
        var paymentResult = await _paymentService.ProcessPaymentAsync(order.TotalAmount, paymentMethod);
        if (!paymentResult.IsSuccess)
            throw new InvalidOperationException($"Payment failed: {paymentResult.ErrorMessage}");

        order.Status = OrderStatus.Confirmed;
        
        // Update stock quantities
        foreach (var item in items)
        {
            await _productRepository.UpdateStockAsync(item.ProductId, 
                item.Product.StockQuantity - item.Quantity);
        }

        var createdOrder = await _orderRepository.CreateAsync(order);
        
        // Send confirmation
        await _notificationService.SendOrderConfirmationAsync(customer.Email, createdOrder);

        return createdOrder;
    }

    public async Task<Order?> GetOrderAsync(int orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task CancelOrderAsync(int orderId, string reason)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order with ID {orderId} not found");

        if (order.Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Cannot cancel delivered order");

        if (order.Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled");

        order.Status = OrderStatus.Cancelled;
        order.Notes = $"Cancelled: {reason}";

        // Restore stock
        foreach (var item in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                await _productRepository.UpdateStockAsync(item.ProductId, 
                    product.StockQuantity + item.Quantity);
            }
        }

        await _orderRepository.UpdateAsync(order);
    }

    private decimal CalculateDiscount(Customer customer, decimal totalAmount)
    {
        return customer.Type switch
        {
            CustomerType.Premium => totalAmount * 0.05m,
            CustomerType.Vip => totalAmount * 0.10m,
            _ => 0m
        };
    }
}