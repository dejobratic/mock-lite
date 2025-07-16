// ReSharper disable MemberCanBePrivate.Global
namespace MockLite.Tests.Unit;

public class SyncVoidMethodsTests
{
    public interface ISyncVoidMethods
    {
        void LogOrderCreation(string customerEmail);
    
        void LogOrderProcessing(string customerEmail, int orderId);
    
        void LogOrderCompletion(string customerEmail, int orderId, decimal total);
    
        void LogShippingUpdate(string customerEmail, int orderId, string trackingNumber, string carrier);
    }
    
    private readonly Mock<ISyncVoidMethods> _sut = new();

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderCreation_ThenMethodCompletes()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.LogOrderCreation(customerEmail));

        // Act & Assert (no exception should be thrown)
        _sut.Object.LogOrderCreation(customerEmail);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderCreationWithThrows_ThenThrowsException()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        
        _sut.Setup(x => x.LogOrderCreation(customerEmail))
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.LogOrderCreation(customerEmail));
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderCreationWithThrowsSpecificException_ThenThrowsSpecificException()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const string exceptionMessage = "Logging service unavailable";
        
        _sut.Setup(x => x.LogOrderCreation(customerEmail))
            .Throws(new InvalidOperationException(exceptionMessage));

        // Act & Assert
        var actual = Assert.Throws<InvalidOperationException>(() => _sut.Object.LogOrderCreation(customerEmail));
        Assert.Equal("Logging service unavailable", actual.Message);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderCreationWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        
        var callbackExecuted = false;
        
        _sut.Setup(x => x.LogOrderCreation(customerEmail))
            .Callback(() => callbackExecuted = true);

        // Act
        _sut.Object.LogOrderCreation(customerEmail);

        // Assert
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderCreationWithDelegateCallback_ThenCallbackExecuted()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        
        var capturedEmail = "";
        
        _sut.Setup(x => x.LogOrderCreation(customerEmail))
            .Callback((string email) => capturedEmail = email);

        // Act
        _sut.Object.LogOrderCreation(customerEmail);

        // Assert
        Assert.Equal(customerEmail, capturedEmail);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderProcessing_ThenMethodCompletes()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        
        _sut.Setup(x => x.LogOrderProcessing(customerEmail, orderId));

        // Act & Assert (no exception should be thrown)
        _sut.Object.LogOrderProcessing(customerEmail, orderId);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderProcessingWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        
        var capturedEmail = "";
        var capturedOrderId = 0;
        
        _sut.Setup(x => x.LogOrderProcessing(customerEmail, orderId))
            .Callback((string email, int id) =>
            {
                capturedEmail = email;
                capturedOrderId = id;
            });

        // Act
        _sut.Object.LogOrderProcessing(customerEmail, orderId);

        // Assert
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(orderId, capturedOrderId);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderCompletion_ThenMethodCompletes()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal total = 150.75m;
        
        _sut.Setup(x => x.LogOrderCompletion(customerEmail, orderId, total));

        // Act & Assert (no exception should be thrown)
        _sut.Object.LogOrderCompletion(customerEmail, orderId, total);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingOrderCompletionWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal total = 150.75m;
        
        var capturedEmail = "";
        var capturedOrderId = 0;
        var capturedTotal = 0m;
        
        _sut.Setup(x => x.LogOrderCompletion(customerEmail, orderId, total))
            .Callback((string email, int id, decimal tot) =>
            {
                capturedEmail = email;
                capturedOrderId = id;
                capturedTotal = tot;
            });

        // Act
        _sut.Object.LogOrderCompletion(customerEmail, orderId, total);

        // Assert
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(total, capturedTotal);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingShippingUpdate_ThenMethodCompletes()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const string trackingNumber = "1Z999AA1234567890";
        const string carrier = "UPS";
        
        _sut.Setup(x => x.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier));

        // Act & Assert (no exception should be thrown)
        _sut.Object.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier);
    }

    [Fact]
    public void GivenValidParameters_WhenLoggingShippingUpdateWithCallback_ThenCallbackExecuted()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const string trackingNumber = "1Z999AA1234567890";
        const string carrier = "UPS";
        
        var capturedEmail = "";
        var capturedOrderId = 0;
        var capturedTrackingNumber = "";
        var capturedCarrier = "";
        
        _sut.Setup(x => x.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier))
            .Callback((string email, int id, string tracking, string car) =>
            {
                capturedEmail = email;
                capturedOrderId = id;
                capturedTrackingNumber = tracking;
                capturedCarrier = car;
            });

        // Act
        _sut.Object.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier);

        // Assert
        Assert.Equal(customerEmail, capturedEmail);
        Assert.Equal(orderId, capturedOrderId);
        Assert.Equal(trackingNumber, capturedTrackingNumber);
        Assert.Equal(carrier, capturedCarrier);
    }

    [Fact]
    public void GivenSequenceSetup_WhenCallingLogOrderCreationMultipleTimes_ThenExecutesSequence()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        
        _sut.SetupSequence(x => x.LogOrderCreation(customerEmail))
            .Callback(() => { /* First call */ })
            .Callback(() => { /* Second call */ })
            .Throws<InvalidOperationException>();

        // Act
        _sut.Object.LogOrderCreation(customerEmail);
        _sut.Object.LogOrderCreation(customerEmail);

        // Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.LogOrderCreation(customerEmail));
    }

    [Fact]
    public void GivenSequenceSetupWithCallback_WhenCallingLogOrderProcessing_ThenCallbackExecuted()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.LogOrderProcessing(customerEmail, orderId))
            .Callback(() => { /* First call */ })
            .Callback(() => callbackExecuted = true);

        // Act
        _sut.Object.LogOrderProcessing(customerEmail, orderId);
        _sut.Object.LogOrderProcessing(customerEmail, orderId);

        // Assert
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void GivenMixedSequenceSetup_WhenCallingLogOrderCompletion_ThenExecutesSequence()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal total = 150.75m;
        
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.LogOrderCompletion(customerEmail, orderId, total))
            .Callback(() => { /* First call */ })
            .Callback(() => callbackExecuted = true)
            .Throws<InvalidOperationException>();

        // Act
        _sut.Object.LogOrderCompletion(customerEmail, orderId, total);
        _sut.Object.LogOrderCompletion(customerEmail, orderId, total);

        // Assert
        Assert.True(callbackExecuted);
        Assert.Throws<InvalidOperationException>(() => _sut.Object.LogOrderCompletion(customerEmail, orderId, total));
    }

    [Fact]
    public void GivenNoSetup_WhenCallingLogOrderCreation_ThenMethodCompletes()
    {
        // Arrange & Act & Assert (no exception should be thrown)
        _sut.Object.LogOrderCreation("customer@example.com");
    }

    [Fact]
    public void GivenNoSetup_WhenCallingLogOrderProcessing_ThenMethodCompletes()
    {
        // Arrange & Act & Assert (no exception should be thrown)
        _sut.Object.LogOrderProcessing("customer@example.com", 12345);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingLogOrderCompletion_ThenMethodCompletes()
    {
        // Arrange & Act & Assert (no exception should be thrown)
        _sut.Object.LogOrderCompletion("customer@example.com", 12345, 150.75m);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingLogShippingUpdate_ThenMethodCompletes()
    {
        // Arrange & Act & Assert (no exception should be thrown)
        _sut.Object.LogShippingUpdate("customer@example.com", 12345, "1Z999AA1234567890", "UPS");
    }

    [Fact]
    public void GivenLogOrderCreationCalled_WhenVerifyingOnce_ThenVerificationSucceeds()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        
        // Act
        _sut.Object.LogOrderCreation(customerEmail);

        // Assert
        _sut.Verify(x => x.LogOrderCreation(customerEmail), Times.Once);
    }

    [Fact]
    public void GivenLogOrderProcessingNotCalled_WhenVerifyingNever_ThenVerificationSucceeds()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;

        // Act
        // No call made

        // Assert
        _sut.Verify(x => x.LogOrderProcessing(customerEmail, orderId), Times.Never);
    }

    [Fact]
    public void GivenLogOrderCompletionCalledTwice_WhenVerifyingExactly_ThenVerificationSucceeds()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal total = 150.75m;
        
        // Act
        _sut.Object.LogOrderCompletion(customerEmail, orderId, total);
        _sut.Object.LogOrderCompletion(customerEmail, orderId, total);

        // Assert
        _sut.Verify(
            x => x.LogOrderCompletion(customerEmail, orderId, total),
            Times.Exactly(2));
    }

    [Fact]
    public void GivenLogShippingUpdateCalledMultipleTimes_WhenVerifyingAtLeast_ThenVerificationSucceeds()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const string trackingNumber = "1Z999AA1234567890";
        const string carrier = "UPS";
        
        // Act
        _sut.Object.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier);
        _sut.Object.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier);
        _sut.Object.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier);

        // Assert
        _sut.Verify(
            x => x.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier),
            Times.AtLeast(2));
    }

    [Fact]
    public void GivenLogOrderCreationCalledOnce_WhenVerifyingAtMost_ThenVerificationSucceeds()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        
        // Act
        _sut.Object.LogOrderCreation(customerEmail);

        // Assert
        _sut.Verify(
            x => x.LogOrderCreation(customerEmail),
            Times.AtMost(2));
    }

    [Fact]
    public void GivenLogOrderProcessingCalledTwice_WhenVerifyingBetween_ThenVerificationSucceeds()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        
        // Act
        _sut.Object.LogOrderProcessing(customerEmail, orderId);
        _sut.Object.LogOrderProcessing(customerEmail, orderId);

        // Assert
        _sut.Verify(
            x => x.LogOrderProcessing(customerEmail, orderId),
            Times.Between(1, 3));
    }

    [Fact]
    public void GivenComplexScenario_WhenCallingMultipleMethods_ThenAllBehaviorsWork()
    {
        // Arrange
        const string customerEmail = "customer@example.com";
        const int orderId = 12345;
        const decimal total = 150.75m;
        const string trackingNumber = "1Z999AA1234567890";
        const string carrier = "UPS";
        
        var creationCallbackExecuted = false;
        var completionCallbackExecuted = false;
        
        _sut.Setup(x => x.LogOrderCreation(customerEmail))
            .Callback(() => creationCallbackExecuted = true);
            
        _sut.Setup(x => x.LogOrderProcessing(customerEmail, orderId));
            
        _sut.Setup(x => x.LogOrderCompletion(customerEmail, orderId, total))
            .Callback(() => completionCallbackExecuted = true);
            
        _sut.Setup(x => x.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier))
            .Throws<InvalidOperationException>();

        // Act
        _sut.Object.LogOrderCreation(customerEmail);
        _sut.Object.LogOrderProcessing(customerEmail, orderId);
        _sut.Object.LogOrderCompletion(customerEmail, orderId, total);

        // Assert
        Assert.True(creationCallbackExecuted);
        Assert.True(completionCallbackExecuted);
        Assert.Throws<InvalidOperationException>(() => _sut.Object.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier));
        
        // Verify all calls
        _sut.Verify(x => x.LogOrderCreation(customerEmail), Times.Once);
        _sut.Verify(x => x.LogOrderProcessing(customerEmail, orderId), Times.Once);
        _sut.Verify(x => x.LogOrderCompletion(customerEmail, orderId, total), Times.Once);
        _sut.Verify(x => x.LogShippingUpdate(customerEmail, orderId, trackingNumber, carrier), Times.Once);
    }
}