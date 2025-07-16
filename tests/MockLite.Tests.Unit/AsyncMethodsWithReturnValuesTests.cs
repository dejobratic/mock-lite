// ReSharper disable MemberCanBePrivate.Global
namespace MockLite.Tests.Unit;

public class AsyncMethodsWithReturnValuesTests
{
    public interface IAsyncMethodsWithReturnValues
    {
        Task<decimal> CalculateOrderTotalAsync(int orderId, string customerEmail);
    
        Task<bool> ProcessOrderAsync(int orderId, string customerEmail, decimal amount, string paymentMethod);
    
        Task<string> GetOrderStatusAsync(int orderId, string customerEmail, bool includeTracking);
    
        Task<bool> ValidateOrderAsync(int orderId, string customerEmail, string shippingAddress, string billingAddress);
    }

    private readonly Mock<IAsyncMethodsWithReturnValues> _sut = new();

    [Fact]
    public async Task GivenValidParameters_WhenCalculatingOrderTotal_ThenReturnsCorrectAmount()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal expectedTotal = 150.75m;
        
        _sut.Setup(x => x.CalculateOrderTotalAsync(orderId, customerEmail))
            .ReturnsAsync(expectedTotal);

        // Act
        var actual = await _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail);

        // Assert
        Assert.Equal(expectedTotal, actual);
    }

    [Fact]
    public async Task GivenValidParameters_WhenCalculatingOrderTotalWithThrows_ThenThrowsException()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.CalculateOrderTotalAsync(orderId, customerEmail))
            .Throws<InvalidOperationException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail));
    }

    [Fact]
    public async Task GivenValidParameters_WhenCalculatingOrderTotalWithThrowsSpecificException_ThenThrowsSpecificException()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string exceptionMessage = "Order not found";
        
        _sut.Setup(x => x.CalculateOrderTotalAsync(orderId, customerEmail))
            .Throws(new ArgumentException(exceptionMessage));

        // Act & Assert
        var actual = await Assert.ThrowsAsync<ArgumentException>(() => _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail));
        Assert.Equal("Order not found", actual.Message);
    }

    [Fact]
    public async Task GivenValidParameters_WhenCalculatingOrderTotalWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        var callbackExecuted = false;
        
        _sut.Setup(x => x.CalculateOrderTotalAsync(orderId, customerEmail))
            .Callback(() => callbackExecuted = true)
            .ReturnsAsync(150.75m);

        // Act
        var actual = await _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail);

        // Assert
        Assert.True(callbackExecuted);
        Assert.Equal(150.75m, actual);
    }

    [Fact]
    public async Task GivenValidParameters_WhenCalculatingOrderTotalWithDelegateCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        var capturedOrderId = 0;
        var capturedEmail = "";
        
        _sut.Setup(x => x.CalculateOrderTotalAsync(orderId, customerEmail))
            .Callback((int id, string email) =>
            {
                capturedOrderId = id;
                capturedEmail = email;
            })
            .ReturnsAsync(150.75m);

        // Act
        var actual = await _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(150.75m, actual);
    }

    [Fact]
    public async Task GivenValidParameters_WhenProcessingOrder_ThenReturnsTrue()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal amount = 150.75m;
        const string paymentMethod = "Credit Card";
        
        _sut.Setup(x => x.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod))
            .ReturnsAsync(true);

        // Act
        var actual = await _sut.Object.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task GivenInvalidParameters_WhenProcessingOrder_ThenReturnsFalse()
    {
        // Arrange
        const int orderId = 99999;
        const string customerEmail = "invalid@example.com";
        const decimal amount = -1.0m;
        const string paymentMethod = "Invalid";
        
        _sut.Setup(x => x.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod))
            .ReturnsAsync(false);

        // Act
        var actual = await _sut.Object.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public async Task GivenValidParameters_WhenProcessingOrderWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal amount = 150.75m;
        const string paymentMethod = "Credit Card";
        
        var capturedOrderId = 0;
        var capturedEmail = "";
        var capturedAmount = 0m;
        var capturedPaymentMethod = "";
        
        _sut.Setup(x => x.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod))
            .Callback((int id, string email, decimal amt, string payment) =>
            {
                capturedOrderId = id;
                capturedEmail = email;
                capturedAmount = amt;
                capturedPaymentMethod = payment;
            })
            .ReturnsAsync(true);

        // Act
        var actual = await _sut.Object.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(amount, capturedAmount);
        Assert.Equal(paymentMethod, capturedPaymentMethod);
        Assert.True(actual);
    }

    [Fact]
    public async Task GivenValidParameters_WhenGettingOrderStatus_ThenReturnsCorrectStatus()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const bool includeTracking = true;
        const string expectedStatus = "Shipped - Tracking: 1Z999AA1234567890";
        
        _sut.Setup(x => x.GetOrderStatusAsync(orderId, customerEmail, includeTracking))
            .ReturnsAsync(expectedStatus);

        // Act
        var actual = await _sut.Object.GetOrderStatusAsync(orderId, customerEmail, includeTracking);

        // Assert
        Assert.Equal(expectedStatus, actual);
    }

    [Fact]
    public async Task GivenValidParameters_WhenGettingOrderStatusWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const bool includeTracking = true;
        
        var capturedOrderId = 0;
        var capturedEmail = "";
        var capturedIncludeTracking = false;
        
        _sut.Setup(x => x.GetOrderStatusAsync(orderId, customerEmail, includeTracking))
            .Callback((int id, string email, bool tracking) =>
            {
                capturedOrderId = id;
                capturedEmail = email;
                capturedIncludeTracking = tracking;
            })
            .ReturnsAsync("Order processed");

        // Act
        var actual = await _sut.Object.GetOrderStatusAsync(orderId, customerEmail, includeTracking);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(includeTracking, capturedIncludeTracking);
        Assert.Equal("Order processed", actual);
    }

    [Fact]
    public async Task GivenValidParameters_WhenValidatingOrder_ThenReturnsTrue()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string shippingAddress = "123 Main St, Anytown, USA";
        const string billingAddress = "456 Oak Ave, Somewhere, USA";
        
        _sut.Setup(x => x.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress))
            .ReturnsAsync(true);

        // Act
        var actual = await _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task GivenInvalidParameters_WhenValidatingOrder_ThenReturnsFalse()
    {
        // Arrange
        const int orderId = 99999;
        const string customerEmail = "invalid@example.com";
        const string shippingAddress = "";
        const string billingAddress = "";
        
        _sut.Setup(x => x.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress))
            .ReturnsAsync(false);

        // Act
        var actual = await _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public async Task GivenValidParameters_WhenValidatingOrderWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string shippingAddress = "123 Main St, Anytown, USA";
        const string billingAddress = "456 Oak Ave, Somewhere, USA";
        
        var capturedOrderId = 0;
        var capturedEmail = "";
        var capturedShippingAddress = "";
        var capturedBillingAddress = "";
        
        _sut.Setup(x => x.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress))
            .Callback((int id, string email, string shipping, string billing) =>
            {
                capturedOrderId = id;
                capturedEmail = email;
                capturedShippingAddress = shipping;
                capturedBillingAddress = billing;
            })
            .ReturnsAsync(true);

        // Act
        var actual = await _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(shippingAddress, capturedShippingAddress);
        Assert.Equal(billingAddress, capturedBillingAddress);
        Assert.True(actual);
    }

    [Fact]
    public async Task GivenSequenceSetup_WhenCallingCalculateOrderTotalMultipleTimes_ThenReturnsSequenceValues()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.SetupSequence(x => x.CalculateOrderTotalAsync(orderId, customerEmail))
            .ReturnsAsync(150.75m)
            .ReturnsAsync(175.25m)
            .ReturnsAsync(200.00m);

        // Act
        var firstCall = await _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail);
        var secondCall = await _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail);
        var thirdCall = await _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail);

        // Assert
        Assert.Equal(150.75m, firstCall);
        Assert.Equal(175.25m, secondCall);
        Assert.Equal(200.00m, thirdCall);
    }

    [Fact]
    public async Task GivenSequenceSetupWithThrows_WhenCallingProcessOrder_ThenThrowsException()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal amount = 150.75m;
        const string paymentMethod = "Credit Card";
        
        _sut.SetupSequence(x => x.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod))
            .ReturnsAsync(true)
            .Throws<InvalidOperationException>();

        // Act
        var firstCall = await _sut.Object.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod);
        
        // Assert
        Assert.True(firstCall);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Object.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod));
    }

    [Fact]
    public async Task GivenSequenceSetupWithCallback_WhenCallingGetOrderStatus_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const bool includeTracking = true;
        
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.GetOrderStatusAsync(orderId, customerEmail, includeTracking))
            .ReturnsAsync("Processing")
            .Callback(() => callbackExecuted = true)
            .ReturnsAsync("Shipped");

        // Act
        var firstCall = await _sut.Object.GetOrderStatusAsync(orderId, customerEmail, includeTracking);
        var secondCall = await _sut.Object.GetOrderStatusAsync(orderId, customerEmail, includeTracking);

        // Assert
        Assert.Equal("Processing", firstCall);
        Assert.True(callbackExecuted);
        Assert.Equal("Shipped", secondCall);
    }

    [Fact]
    public async Task GivenMixedSequenceSetup_WhenCallingValidateOrder_ThenExecutesSequence()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string shippingAddress = "123 Main St, Anytown, USA";
        const string billingAddress = "456 Oak Ave, Somewhere, USA";
        
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress))
            .ReturnsAsync(true)
            .Callback(() => callbackExecuted = true)
            .ReturnsAsync(false)
            .Throws<InvalidOperationException>();

        // Act
        var firstCall = await _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress);
        var secondCall = await _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress);

        // Assert
        Assert.True(firstCall);
        Assert.True(callbackExecuted);
        Assert.False(secondCall);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress));
    }

    [Fact]
    public async Task GivenNoSetup_WhenCallingCalculateOrderTotal_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = await _sut.Object.CalculateOrderTotalAsync(12345, "customer@example.com");

        // Assert
        Assert.Equal(0m, actual);
    }

    [Fact]
    public async Task GivenNoSetup_WhenCallingProcessOrder_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = await _sut.Object.ProcessOrderAsync(12345, "customer@example.com", 150.75m, "Credit Card");

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public async Task GivenNoSetup_WhenCallingGetOrderStatus_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = await _sut.Object.GetOrderStatusAsync(12345, "customer@example.com", true);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task GivenNoSetup_WhenCallingValidateOrder_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = await _sut.Object.ValidateOrderAsync(12345, "customer@example.com", "123 Main St", "456 Oak Ave");

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public async Task GivenCalculateOrderTotalCalled_WhenVerifyingOnce_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.CalculateOrderTotalAsync(orderId, customerEmail))
            .ReturnsAsync(150.75m);

        // Act
        await _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail);

        // Assert
        _sut.Verify(x => x.CalculateOrderTotalAsync(orderId, customerEmail), Times.Once);
    }

    [Fact]
    public void GivenProcessOrderNotCalled_WhenVerifyingNever_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal amount = 150.75m;
        const string paymentMethod = "Credit Card";

        // Act
        // No call made

        // Assert
        _sut.Verify(x => x.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod), Times.Never);
    }

    [Fact]
    public async Task GivenGetOrderStatusCalledTwice_WhenVerifyingExactly_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const bool includeTracking = true;
        
        _sut.Setup(x => x.GetOrderStatusAsync(orderId, customerEmail, includeTracking))
            .ReturnsAsync("Processing");

        // Act
        await _sut.Object.GetOrderStatusAsync(orderId, customerEmail, includeTracking);
        await _sut.Object.GetOrderStatusAsync(orderId, customerEmail, includeTracking);

        // Assert
        _sut.Verify(
            x => x.GetOrderStatusAsync(orderId, customerEmail, includeTracking),
            Times.Exactly(2));
    }

    [Fact]
    public async Task GivenValidateOrderCalledMultipleTimes_WhenVerifyingAtLeast_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string shippingAddress = "123 Main St, Anytown, USA";
        const string billingAddress = "456 Oak Ave, Somewhere, USA";
        
        _sut.Setup(x => x.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress))
            .ReturnsAsync(true);

        // Act
        await _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress);
        await _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress);
        await _sut.Object.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress);

        // Assert
        _sut.Verify(
            x => x.ValidateOrderAsync(orderId, customerEmail, shippingAddress, billingAddress),
            Times.AtLeast(2));
    }

    [Fact]
    public async Task GivenCalculateOrderTotalCalledOnce_WhenVerifyingAtMost_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.CalculateOrderTotalAsync(orderId, customerEmail))
            .ReturnsAsync(150.75m);

        // Act
        await _sut.Object.CalculateOrderTotalAsync(orderId, customerEmail);

        // Assert
        _sut.Verify(
            x => x.CalculateOrderTotalAsync(orderId, customerEmail),
            Times.AtMost(2));
    }

    [Fact]
    public async Task GivenProcessOrderCalledTwice_WhenVerifyingBetween_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal amount = 150.75m;
        const string paymentMethod = "Credit Card";
        
        _sut.Setup(x => x.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod))
            .ReturnsAsync(true);

        // Act
        await _sut.Object.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod);
        await _sut.Object.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod);

        // Assert
        _sut.Verify(
            x => x.ProcessOrderAsync(orderId, customerEmail, amount, paymentMethod),
            Times.Between(1, 3));
    }
}