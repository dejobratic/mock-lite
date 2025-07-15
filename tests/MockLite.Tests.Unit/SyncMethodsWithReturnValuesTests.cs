namespace MockLite.Tests.Unit;

public class SyncMethodsWithReturnValuesTests
{
    private interface ISyncMethodsWithReturnValues
    {
        bool ValidateOrder(int orderId);
    
        decimal CalculateShipping(decimal weight, string destination);
    
        decimal CalculateTax(int orderId, string customerState, decimal subtotal);
    
        string GenerateOrderConfirmation(int orderId, string customerEmail, decimal total, string paymentMethod);
    }
    
    private readonly Mock<ISyncMethodsWithReturnValues> _sut = new();

    [Fact]
    public void GivenValidOrderId_WhenValidatingOrder_ThenReturnsTrue()
    {
        // Arrange
        const int orderId = 12345;

        _sut.Setup(x => x.ValidateOrder(orderId))
            .Returns(true);

        // Act
        var actual = _sut.Object.ValidateOrder(orderId);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenInvalidOrderId_WhenValidatingOrder_ThenReturnsFalse()
    {
        // Arrange
        const int orderId = 99999;

        _sut.Setup(x => x.ValidateOrder(orderId))
            .Returns(false);

        // Act
        var actual = _sut.Object.ValidateOrder(orderId);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void GivenValidOrderId_WhenValidatingOrderWithThrows_ThenThrowsException()
    {
        // Arrange
        const int orderId = 12345;
        
        _sut.Setup(x => x.ValidateOrder(orderId))
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.ValidateOrder(orderId));
    }

    [Fact]
    public void GivenValidOrderId_WhenValidatingOrderWithThrowsSpecificException_ThenThrowsSpecificException()
    {
        // Arrange
        const int orderId = 12345;
        const string exceptionMessage = "Invalid order ID";
        
        _sut.Setup(x => x.ValidateOrder(orderId))
            .Throws(new ArgumentException(exceptionMessage));

        // Act & Assert
        var actual = Assert.Throws<ArgumentException>(() => _sut.Object.ValidateOrder(orderId));
        Assert.Equal("Invalid order ID", actual.Message);
    }

    [Fact]
    public void GivenValidOrderId_WhenValidatingOrderWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        
        var callbackExecuted = false;
        
        _sut.Setup(x => x.ValidateOrder(orderId))
            .Callback(() => callbackExecuted = true)
            .Returns(true);

        // Act
        var actual = _sut.Object.ValidateOrder(orderId);

        // Assert
        Assert.True(callbackExecuted);
        Assert.True(actual);
    }

    [Fact]
    public void GivenValidOrderId_WhenValidatingOrderWithDelegateCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        
        var capturedOrderId = 0;
        
        _sut.Setup(x => x.ValidateOrder(orderId))
            .Callback((int id) => capturedOrderId = id)
            .Returns(true);

        // Act
        var actual = _sut.Object.ValidateOrder(orderId);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.True(actual);
    }

    [Fact]
    public void GivenValidParameters_WhenCalculatingShipping_ThenReturnsCorrectAmount()
    {
        // Arrange
        const decimal weight = 2.5m;
        const string destination = "New York";
        const decimal expectedShipping = 15.99m;
        
        _sut.Setup(x => x.CalculateShipping(weight, destination))
            .Returns(expectedShipping);

        // Act
        var actual = _sut.Object.CalculateShipping(weight, destination);

        // Assert
        Assert.Equal(expectedShipping, actual);
    }

    [Fact]
    public void GivenValidParameters_WhenCalculatingShippingWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const decimal weight = 2.5m;
        const string destination = "New York";
        
        var capturedWeight = 0m;
        var capturedDestination = "";
        
        _sut.Setup(x => x.CalculateShipping(weight, destination))
            .Callback((decimal w, string d) =>
            {
                capturedWeight = w;
                capturedDestination = d;
            })
            .Returns(15.99m);

        // Act
        var actual = _sut.Object.CalculateShipping(weight, destination);

        // Assert
        Assert.Equal(weight, capturedWeight);
        Assert.Equal(destination, capturedDestination);
        Assert.Equal(15.99m, actual);
    }

    [Fact]
    public void GivenValidParameters_WhenCalculatingTax_ThenReturnsCorrectAmount()
    {
        // Arrange
        const int orderId = 12345;
        const string customerState = "CA";
        const decimal subtotal = 100.00m;
        const decimal expectedTax = 8.25m;
        
        _sut.Setup(x => x.CalculateTax(orderId, customerState, subtotal))
            .Returns(expectedTax);

        // Act
        var actual = _sut.Object.CalculateTax(orderId, customerState, subtotal);

        // Assert
        Assert.Equal(expectedTax, actual);
    }

    [Fact]
    public void GivenValidParameters_WhenCalculatingTaxWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerState = "CA";
        const decimal subtotal = 100.00m;
        
        var capturedOrderId = 0;
        var capturedState = "";
        var capturedSubtotal = 0m;
        
        _sut.Setup(x => x.CalculateTax(orderId, customerState, subtotal))
            .Callback((int id, string state, decimal sub) =>
            {
                capturedOrderId = id;
                capturedState = state;
                capturedSubtotal = sub;
            })
            .Returns(8.25m);

        // Act
        var actual = _sut.Object.CalculateTax(orderId, customerState, subtotal);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerState, capturedState);
        Assert.Equal(subtotal, capturedSubtotal);
        Assert.Equal(8.25m, actual);
    }

    [Fact]
    public void GivenValidParameters_WhenGeneratingOrderConfirmation_ThenReturnsCorrectString()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal total = 150.00m;
        const string paymentMethod = "Credit Card";
        const string expectedConfirmation = "Order #12345 confirmed for customer@example.com";
        
        _sut.Setup(x => x.GenerateOrderConfirmation(orderId, customerEmail, total, paymentMethod))
            .Returns(expectedConfirmation);

        // Act
        var actual = _sut.Object.GenerateOrderConfirmation(orderId, customerEmail, total, paymentMethod);

        // Assert
        Assert.Equal(expectedConfirmation, actual);
    }

    [Fact]
    public void GivenValidParameters_WhenGeneratingOrderConfirmationWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal total = 150.00m;
        const string paymentMethod = "Credit Card";
        
        var capturedOrderId = 0;
        var capturedEmail = "";
        var capturedTotal = 0m;
        var capturedPaymentMethod = "";
        
        _sut.Setup(x => x.GenerateOrderConfirmation(orderId, customerEmail, total, paymentMethod))
            .Callback((int id, string email, decimal tot, string payment) =>
            {
                capturedOrderId = id;
                capturedEmail = email;
                capturedTotal = tot;
                capturedPaymentMethod = payment;
            })
            .Returns("Order confirmed");

        // Act
        var actual = _sut.Object.GenerateOrderConfirmation(orderId, customerEmail, total, paymentMethod);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(total, capturedTotal);
        Assert.Equal(paymentMethod, capturedPaymentMethod);
        Assert.Equal("Order confirmed", actual);
    }

    [Fact]
    public void GivenSequenceSetup_WhenCallingValidateOrderMultipleTimes_ThenReturnsSequenceValues()
    {
        // Arrange
        const int orderId = 12345;
        
        _sut.SetupSequence(x => x.ValidateOrder(orderId))
            .Returns(true)
            .Returns(false)
            .Returns(true);

        // Act
        var firstCall = _sut.Object.ValidateOrder(orderId);
        var secondCall = _sut.Object.ValidateOrder(orderId);
        var thirdCall = _sut.Object.ValidateOrder(orderId);

        // Assert
        Assert.True(firstCall);
        Assert.False(secondCall);
        Assert.True(thirdCall);
    }

    [Fact]
    public void GivenSequenceSetupWithThrows_WhenCallingValidateOrder_ThenThrowsException()
    {
        // Arrange
        const int orderId = 12345;
        
        _sut.SetupSequence(x => x.ValidateOrder(orderId))
            .Returns(true)
            .Throws<InvalidOperationException>();

        // Act
        var firstCall = _sut.Object.ValidateOrder(orderId);
        
        // Assert
        Assert.True(firstCall);
        Assert.Throws<InvalidOperationException>(() => _sut.Object.ValidateOrder(orderId));
    }

    [Fact]
    public void GivenSequenceSetupWithCallback_WhenCallingValidateOrder_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.ValidateOrder(orderId))
            .Returns(true)
            .Callback(() => callbackExecuted = true)
            .Returns(false);

        // Act
        var firstCall = _sut.Object.ValidateOrder(orderId);
        var secondCall = _sut.Object.ValidateOrder(orderId);

        // Assert
        Assert.True(firstCall);
        Assert.True(callbackExecuted);
        Assert.False(secondCall);
    }

    [Fact]
    public void GivenMixedSequenceSetup_WhenCallingCalculateShipping_ThenExecutesSequence()
    {
        // Arrange
        const decimal weight = 2.5m;
        const string destination = "New York";
        
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.CalculateShipping(weight, destination))
            .Returns(15.99m)
            .Callback(() => callbackExecuted = true)
            .Returns(12.99m)
            .Throws<InvalidOperationException>();

        // Act
        var firstCall = _sut.Object.CalculateShipping(weight, destination);
        var secondCall = _sut.Object.CalculateShipping(weight, destination);

        // Assert
        Assert.Equal(15.99m, firstCall);
        Assert.True(callbackExecuted);
        Assert.Equal(12.99m, secondCall);
        Assert.Throws<InvalidOperationException>(() => _sut.Object.CalculateShipping(weight, destination));
    }

    [Fact]
    public void GivenNoSetup_WhenCallingValidateOrder_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.ValidateOrder(12345);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingCalculateShipping_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.CalculateShipping(2.5m, "New York");

        // Assert
        Assert.Equal(0m, actual);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingGenerateOrderConfirmation_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.GenerateOrderConfirmation(12345, "test@example.com", 100m, "Credit Card");

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GivenValidateOrderCalled_WhenVerifyingOnce_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        
        _sut.Setup(x => x.ValidateOrder(orderId))
            .Returns(true);

        // Act
        _sut.Object.ValidateOrder(orderId);

        // Assert
        _sut.Verify(x => x.ValidateOrder(orderId), Times.Once);
    }

    [Fact]
    public void GivenValidateOrderNotCalled_WhenVerifyingNever_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;

        // Act
        // No call made

        // Assert
        _sut.Verify(x => x.ValidateOrder(orderId), Times.Never);
    }

    [Fact]
    public void GivenValidateOrderCalledTwice_WhenVerifyingExactly_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        
        _sut.Setup(x => x.ValidateOrder(orderId))
            .Returns(true);

        // Act
        _sut.Object.ValidateOrder(orderId);
        _sut.Object.ValidateOrder(orderId);

        // Assert
        _sut.Verify(
            x => x.ValidateOrder(orderId),
            Times.Exactly(2));
    }

    [Fact]
    public void GivenCalculateShippingCalledMultipleTimes_WhenVerifyingAtLeast_ThenVerificationSucceeds()
    {
        // Arrange
        const decimal weight = 2.5m;
        const string destination = "New York";
        _sut.Setup(x => x.CalculateShipping(weight, destination)).Returns(15.99m);

        // Act
        _sut.Object.CalculateShipping(weight, destination);
        _sut.Object.CalculateShipping(weight, destination);
        _sut.Object.CalculateShipping(weight, destination);

        // Assert
        _sut.Verify(
            x => x.CalculateShipping(weight, destination),
            Times.AtLeast(2));
    }

    [Fact]
    public void GivenCalculateTaxCalledOnce_WhenVerifyingAtMost_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerState = "CA";
        const decimal subtotal = 100.00m;
        
        _sut.Setup(x => x.CalculateTax(orderId, customerState, subtotal))
            .Returns(8.25m);

        // Act
        _sut.Object.CalculateTax(orderId, customerState, subtotal);

        // Assert
        _sut.Verify(
            x => x.CalculateTax(orderId, customerState, subtotal),
            Times.AtMost(2));
    }

    [Fact]
    public void GivenGenerateOrderConfirmationCalledTwice_WhenVerifyingBetween_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const decimal total = 150.00m;
        const string paymentMethod = "Credit Card";
        
        _sut.Setup(x => x.GenerateOrderConfirmation(orderId, customerEmail, total, paymentMethod))
            .Returns("Order confirmed");

        // Act
        _sut.Object.GenerateOrderConfirmation(orderId, customerEmail, total, paymentMethod);
        _sut.Object.GenerateOrderConfirmation(orderId, customerEmail, total, paymentMethod);

        // Assert
        _sut.Verify(
            x => x.GenerateOrderConfirmation(orderId, customerEmail, total, paymentMethod),
            Times.Between(1, 3));
    }
}