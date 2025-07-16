// ReSharper disable MemberCanBePrivate.Global
namespace MockLite.Tests.Unit;

public class AsyncVoidMethodsTests
{
    public interface IAsyncVoidMethods
    {
        Task SendOrderConfirmationAsync(int orderId, string customerEmail);
    
        Task LogOrderProcessingAsync(string customerEmail, int orderId, decimal amount);
    
        Task UpdateInventoryAsync(int orderId, string customerEmail, string warehouseId);
    
        Task ScheduleFulfillmentAsync(int orderId, string customerEmail, string shippingAddress, DateTime requestedDate);
    }
    
    private readonly Mock<IAsyncVoidMethods> _sut = new();

    [Fact]
    public async Task GivenValidParameters_WhenSendingOrderConfirmation_ThenTaskCompletes()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.SendOrderConfirmationAsync(orderId, customerEmail))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail);
    }

    [Fact]
    public async Task GivenValidParameters_WhenSendingOrderConfirmationWithThrows_ThenThrowsException()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.SendOrderConfirmationAsync(orderId, customerEmail))
            .Throws<InvalidOperationException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail));
    }

    [Fact]
    public async Task GivenValidParameters_WhenSendingOrderConfirmationWithThrowsSpecificException_ThenThrowsSpecificException()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string exceptionMessage = "Email service unavailable";
        
        _sut.Setup(x => x.SendOrderConfirmationAsync(orderId, customerEmail))
            .Throws(new InvalidOperationException(exceptionMessage));

        // Act & Assert
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail));
        Assert.Equal("Email service unavailable", actual.Message);
    }

    [Fact]
    public async Task GivenValidParameters_WhenSendingOrderConfirmationWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        var callbackExecuted = false;
        
        _sut.Setup(x => x.SendOrderConfirmationAsync(orderId, customerEmail))
            .Callback(() => callbackExecuted = true)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail);

        // Assert
        Assert.True(callbackExecuted);
    }

    [Fact]
    public async Task GivenValidParameters_WhenSendingOrderConfirmationWithDelegateCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        var capturedOrderId = 0;
        var capturedEmail = "";
        
        _sut.Setup(x => x.SendOrderConfirmationAsync(orderId, customerEmail))
            .Callback((int id, string email) =>
            {
                capturedOrderId = id;
                capturedEmail = email;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerEmail, capturedEmail);
    }

    [Fact]
    public async Task GivenValidParameters_WhenLoggingOrderProcessing_ThenTaskCompletes()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal amount = 150.75m;
        
        _sut.Setup(x => x.LogOrderProcessingAsync(customerEmail, orderId, amount))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await _sut.Object.LogOrderProcessingAsync(customerEmail, orderId, amount);
    }

    [Fact]
    public async Task GivenValidParameters_WhenLoggingOrderProcessingWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal amount = 150.75m;
        
        var capturedEmail = "";
        var capturedOrderId = 0;
        var capturedAmount = 0m;
        
        _sut.Setup(x => x.LogOrderProcessingAsync(customerEmail, orderId, amount))
            .Callback((string email, int id, decimal amt) =>
            {
                capturedEmail = email;
                capturedOrderId = id;
                capturedAmount = amt;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.LogOrderProcessingAsync(customerEmail, orderId, amount);

        // Assert
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(amount, capturedAmount);
    }

    [Fact]
    public async Task GivenValidParameters_WhenUpdatingInventory_ThenTaskCompletes()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string warehouseId = "WH001";
        
        _sut.Setup(x => x.UpdateInventoryAsync(orderId, customerEmail, warehouseId))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await _sut.Object.UpdateInventoryAsync(orderId, customerEmail, warehouseId);
    }

    [Fact]
    public async Task GivenValidParameters_WhenUpdatingInventoryWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string warehouseId = "WH001";
        
        var capturedOrderId = 0;
        var capturedEmail = "";
        var capturedWarehouseId = "";
        
        _sut.Setup(x => x.UpdateInventoryAsync(orderId, customerEmail, warehouseId))
            .Callback((int id, string email, string warehouse) =>
            {
                capturedOrderId = id;
                capturedEmail = email;
                capturedWarehouseId = warehouse;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.UpdateInventoryAsync(orderId, customerEmail, warehouseId);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(warehouseId, capturedWarehouseId);
    }

    [Fact]
    public async Task GivenValidParameters_WhenSchedulingFulfillment_ThenTaskCompletes()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string shippingAddress = "123 Main St, Anytown, USA";
        var requestedDate = new DateTime(2025, 7, 20);
        
        _sut.Setup(x => x.ScheduleFulfillmentAsync(orderId, customerEmail, shippingAddress, requestedDate))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await _sut.Object.ScheduleFulfillmentAsync(orderId, customerEmail, shippingAddress, requestedDate);
    }

    [Fact]
    public async Task GivenValidParameters_WhenSchedulingFulfillmentWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string shippingAddress = "123 Main St, Anytown, USA";
        var requestedDate = new DateTime(2025, 7, 20);
        
        var capturedOrderId = 0;
        var capturedEmail = "";
        var capturedAddress = "";
        var capturedDate = DateTime.MinValue;
        
        _sut.Setup(x => x.ScheduleFulfillmentAsync(orderId, customerEmail, shippingAddress, requestedDate))
            .Callback((int id, string email, string address, DateTime date) =>
            {
                capturedOrderId = id;
                capturedEmail = email;
                capturedAddress = address;
                capturedDate = date;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.ScheduleFulfillmentAsync(orderId, customerEmail, shippingAddress, requestedDate);

        // Assert
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(shippingAddress, capturedAddress);
        Assert.Equal(requestedDate, capturedDate);
    }

    [Fact]
    public async Task GivenSequenceSetup_WhenCallingSendOrderConfirmationMultipleTimes_ThenExecutesSequence()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.SetupSequence(x => x.SendOrderConfirmationAsync(orderId, customerEmail))
            .Returns(Task.CompletedTask)
            .Returns(Task.CompletedTask)
            .Throws<InvalidOperationException>();

        // Act
        await _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail);
        await _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail));
    }

    [Fact]
    public async Task GivenSequenceSetupWithCallback_WhenCallingLogOrderProcessing_ThenCallbackExecuted()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal amount = 150.75m;
        
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.LogOrderProcessingAsync(customerEmail, orderId, amount))
            .Returns(Task.CompletedTask)
            .Callback(() => callbackExecuted = true)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.LogOrderProcessingAsync(customerEmail, orderId, amount);
        await _sut.Object.LogOrderProcessingAsync(customerEmail, orderId, amount);

        // Assert
        Assert.True(callbackExecuted);
    }

    [Fact]
    public async Task GivenMixedSequenceSetup_WhenCallingUpdateInventory_ThenExecutesSequence()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string warehouseId = "WH001";
        
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.UpdateInventoryAsync(orderId, customerEmail, warehouseId))
            .Returns(Task.CompletedTask)
            .Callback(() => callbackExecuted = true)
            .Returns(Task.CompletedTask)
            .Throws<InvalidOperationException>();

        // Act
        await _sut.Object.UpdateInventoryAsync(orderId, customerEmail, warehouseId);
        await _sut.Object.UpdateInventoryAsync(orderId, customerEmail, warehouseId);

        // Assert
        Assert.True(callbackExecuted);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Object.UpdateInventoryAsync(orderId, customerEmail, warehouseId));
    }

    [Fact]
    public async Task GivenNoSetup_WhenCallingSendOrderConfirmation_ThenTaskCompletes()
    {
        // Arrange & Act & Assert
        await _sut.Object.SendOrderConfirmationAsync(12345, "customer@example.com");
    }

    [Fact]
    public async Task GivenNoSetup_WhenCallingLogOrderProcessing_ThenTaskCompletes()
    {
        // Arrange & Act & Assert
        await _sut.Object.LogOrderProcessingAsync("customer@example.com", 12345, 150.75m);
    }

    [Fact]
    public async Task GivenNoSetup_WhenCallingUpdateInventory_ThenTaskCompletes()
    {
        // Arrange & Act & Assert
        await _sut.Object.UpdateInventoryAsync(12345, "customer@example.com", "WH001");
    }

    [Fact]
    public async Task GivenNoSetup_WhenCallingScheduleFulfillment_ThenTaskCompletes()
    {
        // Arrange & Act & Assert
        await _sut.Object.ScheduleFulfillmentAsync(12345, "customer@example.com", "123 Main St", new DateTime(2025, 7, 20));
    }

    [Fact]
    public async Task GivenSendOrderConfirmationCalled_WhenVerifyingOnce_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.SendOrderConfirmationAsync(orderId, customerEmail))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail);

        // Assert
        _sut.Verify(x => x.SendOrderConfirmationAsync(orderId, customerEmail), Times.Once);
    }

    [Fact]
    public void GivenLogOrderProcessingNotCalled_WhenVerifyingNever_ThenVerificationSucceeds()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal amount = 150.75m;

        // Act
        // No call made

        // Assert
        _sut.Verify(x => x.LogOrderProcessingAsync(customerEmail, orderId, amount), Times.Never);
    }

    [Fact]
    public async Task GivenUpdateInventoryCalledTwice_WhenVerifyingExactly_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string warehouseId = "WH001";
        
        _sut.Setup(x => x.UpdateInventoryAsync(orderId, customerEmail, warehouseId))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.UpdateInventoryAsync(orderId, customerEmail, warehouseId);
        await _sut.Object.UpdateInventoryAsync(orderId, customerEmail, warehouseId);

        // Assert
        _sut.Verify(
            x => x.UpdateInventoryAsync(orderId, customerEmail, warehouseId),
            Times.Exactly(2));
    }

    [Fact]
    public async Task GivenScheduleFulfillmentCalledMultipleTimes_WhenVerifyingAtLeast_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        const string shippingAddress = "123 Main St, Anytown, USA";
        var requestedDate = new DateTime(2025, 7, 20);

        // Act
        await _sut.Object.ScheduleFulfillmentAsync(orderId, customerEmail, shippingAddress, requestedDate);
        await _sut.Object.ScheduleFulfillmentAsync(orderId, customerEmail, shippingAddress, requestedDate);
        await _sut.Object.ScheduleFulfillmentAsync(orderId, customerEmail, shippingAddress, requestedDate);

        // Assert
        _sut.Verify(
            x => x.ScheduleFulfillmentAsync(orderId, customerEmail, shippingAddress, requestedDate),
            Times.AtLeast(2));
    }

    [Fact]
    public async Task GivenSendOrderConfirmationCalledOnce_WhenVerifyingAtMost_ThenVerificationSucceeds()
    {
        // Arrange
        const int orderId = 12345;
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.SendOrderConfirmationAsync(orderId, customerEmail))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.SendOrderConfirmationAsync(orderId, customerEmail);

        // Assert
        _sut.Verify(
            x => x.SendOrderConfirmationAsync(orderId, customerEmail),
            Times.AtMost(2));
    }

    [Fact]
    public async Task GivenLogOrderProcessingCalledTwice_WhenVerifyingBetween_ThenVerificationSucceeds()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal amount = 150.75m;
        
        _sut.Setup(x => x.LogOrderProcessingAsync(customerEmail, orderId, amount))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Object.LogOrderProcessingAsync(customerEmail, orderId, amount);
        await _sut.Object.LogOrderProcessingAsync(customerEmail, orderId, amount);

        // Assert
        _sut.Verify(
            x => x.LogOrderProcessingAsync(customerEmail, orderId, amount),
            Times.Between(1, 3));
    }
}