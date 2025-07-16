# MockLite

[![NuGet](https://img.shields.io/nuget/v/MockLite.svg)](https://www.nuget.org/packages/MockLite/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

MockLite is a lightweight, fast, and easy-to-use mocking framework for .NET with a fluent interface. It provides essential mocking capabilities without the complexity and overhead of larger frameworks.

## Features

- 🚀 **Lightweight and Fast** - Minimal overhead with dynamic proxy generation
- 🎯 **Simple API** - Clean, fluent interface that's easy to learn and use
- 🔧 **Method Mocking** - Mock methods with return values, callbacks, and exceptions
- 📚 **Sequential Setups** - Define different behaviors for successive calls
- 🏠 **Property Mocking** - Mock property getters and setters
- ⚡ **Async Support** - First-class support for async methods and Tasks
- ✅ **Verification** - Verify method calls with flexible timing constraints
- 🎪 **No Dependencies** - Zero external dependencies

## Installation

Install MockLite via NuGet:

```bash
dotnet add package MockLite
```

Or using the Package Manager Console:

```powershell
Install-Package MockLite
```

## Quick Start

```csharp
using MockLite;

// Create a mock for your interface
var mock = new Mock<IOrderService>();

// Setup method behavior
mock.Setup(x => x.ValidateOrder(12345))
    .Returns(true);

// Use the mock object
var result = mock.Object.ValidateOrder(12345);
Assert.True(result);

// Verify method was called
mock.Verify(x => x.ValidateOrder(12345), Times.Once);
```

## Usage Examples

### Basic Method Mocking

```csharp
public interface IPaymentService
{
    bool ProcessPayment(decimal amount, string cardNumber);
    decimal CalculateFee(decimal amount);
    void LogTransaction(string transactionId);
}

var mock = new Mock<IPaymentService>();

// Mock method with return value
mock.Setup(x => x.ProcessPayment(100.50m, "1234-5678-9012-3456"))
    .Returns(true);

// Mock method that throws exception
mock.Setup(x => x.ProcessPayment(0, "invalid-card"))
    .Throws<ArgumentException>();

// Mock void method with callback
var loggedTransactions = new List<string>();
mock.Setup(x => x.LogTransaction("TXN-123"))
    .Callback<string>(id => loggedTransactions.Add(id));
```

### Property Mocking

```csharp
public interface IConfiguration
{
    string ApiEndpoint { get; set; }
    int Timeout { get; }
}

var mock = new Mock<IConfiguration>();

// Setup property getter
mock.SetupGet(x => x.ApiEndpoint)
    .Returns("https://api.example.com");

// Setup property setter with callback
var setValues = new List<string>();
mock.SetupSet(x => x.ApiEndpoint)
    .Callback((string value) => setValues.Add(value));

// Use the mock
var endpoint = mock.Object.ApiEndpoint; // Returns "https://api.example.com"
mock.Object.ApiEndpoint = "https://new-api.example.com"; // Triggers callback
```

### Sequential Behaviors

For methods that should behave differently on successive calls:

```csharp
public interface IRetryService
{
    bool TryConnect();
    string GetStatus();
}

var mock = new Mock<IRetryService>();

// Setup sequence for retry logic
mock.SetupSequence(x => x.TryConnect())
    .Returns(false)           // First call fails
    .Returns(false)           // Second call fails  
    .Returns(true);           // Third call succeeds

// Setup sequence with mixed behaviors
mock.SetupSequence(x => x.GetStatus())
    .Returns("Connecting")
    .Returns("Retrying")
    .Throws<TimeoutException>();
```

### Async Method Support

```csharp
public interface IAsyncService
{
    Task<string> FetchDataAsync(int id);
    Task ProcessAsync(string data);
}

var mock = new Mock<IAsyncService>();

// Mock async method with return value
mock.Setup(x => x.FetchDataAsync(123))
    .ReturnsAsync("Sample Data");

// Mock async void method
mock.Setup(x => x.ProcessAsync("test"))
    .Returns(Task.CompletedTask);

// Use async setup sequences
mock.SetupSequence(x => x.FetchDataAsync(456))
    .ReturnsAsync("First Result")
    .ReturnsAsync("Second Result")
    .ThrowsAsync<HttpRequestException>();
```

### Verification

Verify that methods were called with expected parameters and frequency:

```csharp
var mock = new Mock<IEmailService>();

// Perform operations
mock.Object.SendEmail("user@example.com", "Subject", "Body");
mock.Object.SendEmail("admin@example.com", "Alert", "Message");

// Verify exact calls
mock.Verify(x => x.SendEmail("user@example.com", "Subject", "Body"), Times.Once);

// Verify total call count (requires setup for each specific call)
mock.Setup(x => x.SendEmail("user@example.com", "Subject", "Body")).Verifiable();
mock.Setup(x => x.SendEmail("admin@example.com", "Alert", "Message")).Verifiable();

// Verify with different constraints
mock.Verify(x => x.SendEmail("user@example.com", "Subject", "Body"), Times.AtLeastOnce);
mock.Verify(x => x.SendEmail("unknown@example.com", "Unknown", "Unknown"), Times.Never);
```

### Callbacks

Execute custom logic when methods are called:

```csharp
public interface IAuditService
{
    void LogAction(string action, string user);
    bool ValidateUser(string username);
}

var mock = new Mock<IAuditService>();
var auditLog = new List<string>();

// Callback with parameters
mock.Setup(x => x.LogAction("login", "john_doe"))
    .Callback((string action, string user) => 
        auditLog.Add($"{DateTime.Now}: {user} performed {action}"));

// Callback with return value
mock.Setup(x => x.ValidateUser("john_doe"))
    .Returns(true)
    .Callback((string username) => Console.WriteLine($"Validating user: {username}"));
```

### Real-World E-commerce Example

```csharp
public interface IOrderService
{
    Task<decimal> CalculateOrderTotalAsync(int orderId);
    Task<bool> ProcessOrderAsync(Order order);
    void NotifyCustomer(string email, string message);
    bool ValidateInventory(int productId, int quantity);
}

public class OrderProcessor
{
    private readonly IOrderService _orderService;

    public OrderProcessor(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<bool> ProcessOrderWithNotificationAsync(Order order)
    {
        // Validate inventory
        if (!_orderService.ValidateInventory(order.ProductId, order.Quantity))
        {
            _orderService.NotifyCustomer(order.CustomerEmail, "Insufficient inventory");
            return false;
        }

        // Calculate total
        var total = await _orderService.CalculateOrderTotalAsync(order.Id);
        order.Total = total;

        // Process order
        var success = await _orderService.ProcessOrderAsync(order);
        
        if (success)
        {
            _orderService.NotifyCustomer(order.CustomerEmail, $"Order confirmed. Total: ${total:F2}");
        }
        else
        {
            _orderService.NotifyCustomer(order.CustomerEmail, "Order processing failed");
        }

        return success;
    }
}

[Test]
public async Task ProcessOrderWithNotificationAsync_WhenInventoryAvailableAndProcessingSucceeds_ShouldNotifySuccess()
{
    // Arrange
    var mock = new Mock<IOrderService>();
    var processor = new OrderProcessor(mock.Object);
    
    var order = new Order 
    { 
        Id = 123, 
        ProductId = 456, 
        Quantity = 2, 
        CustomerEmail = "customer@example.com" 
    };

    var notifications = new List<string>();

    mock.Setup(x => x.ValidateInventory(456, 2))
        .Returns(true);

    mock.Setup(x => x.CalculateOrderTotalAsync(123))
        .ReturnsAsync(99.99m);

    mock.Setup(x => x.ProcessOrderAsync(order))
        .ReturnsAsync(true);

    mock.Setup(x => x.NotifyCustomer("customer@example.com", "Order confirmed. Total: $99.99"))
        .Callback((string email, string message) => notifications.Add(message));

    // Act
    var result = await processor.ProcessOrderWithNotificationAsync(order);

    // Assert
    Assert.True(result);
    Assert.Equal(99.99m, order.Total);
    
    // Verify all interactions
    mock.Verify(x => x.ValidateInventory(456, 2), Times.Once);
    mock.Verify(x => x.CalculateOrderTotalAsync(123), Times.Once);
    mock.Verify(x => x.ProcessOrderAsync(order), Times.Once);
    mock.Verify(x => x.NotifyCustomer("customer@example.com", "Order confirmed. Total: $99.99"), Times.Once);
    
    Assert.Single(notifications);
    Assert.Contains("Order confirmed", notifications[0]);
}
```

## API Reference

### Times Class

Use the `Times` class to specify call count expectations:

- `Times.Never` - Method should never be called
- `Times.Once` - Method should be called exactly once
- `Times.AtLeastOnce` - Method should be called at least once
- `Times.AtMostOnce` - Method should be called at most once
- `Times.Exactly(n)` - Method should be called exactly n times
- `Times.AtLeast(n)` - Method should be called at least n times
- `Times.AtMost(n)` - Method should be called at most n times
- `Times.Between(min, max)` - Method should be called between min and max times

### Setup Methods

- `Setup(expression)` - Setup behavior for method calls
- `SetupGet(expression)` - Setup behavior for property getters
- `SetupSet(expression)` - Setup behavior for property setters
- `SetupSequence(expression)` - Setup sequential behaviors for multiple calls

### Behavior Methods

- `Returns(value)` - Return a specific value
- `Returns(func)` - Return value from function
- `ReturnsAsync(value)` - Return async value (for async methods)
- `Throws<T>()` - Throw exception of type T
- `Throws(exception)` - Throw specific exception instance
- `Callback(action)` - Execute callback when method is called

## Limitations

- Only supports interface mocking (not concrete classes)
- Requires .NET 8.0 or later
- Uses exact parameter matching (no built-in argument matchers like `It.IsAny<T>()`)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.