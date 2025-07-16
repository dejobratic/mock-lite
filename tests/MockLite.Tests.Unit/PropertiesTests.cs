// ReSharper disable MemberCanBePrivate.Global
namespace MockLite.Tests.Unit;

public class PropertiesTests
{
    public interface IProperties
    {
        string CurrentCustomerEmail { get; set; }
    
        int MaxProcessingTimeoutSeconds { get; set; }
    
        bool IsValidationEnabled { get; set; }
    
        string DefaultShippingMethod { get; set; }
    
        decimal CurrentTaxRate { get; }
    
        bool IsServiceOnline { get; }
    }
    
    private readonly Mock<IProperties> _sut = new();

    [Fact]
    public void GivenSetupForCurrentCustomerEmail_WhenGetting_ThenReturnsCorrectValue()
    {
        // Arrange
        const string expectedEmail = "customer@example.com";
        
        _sut.Setup(x => x.CurrentCustomerEmail)
            .Returns(expectedEmail);

        // Act
        var actual = _sut.Object.CurrentCustomerEmail;

        // Assert
        Assert.Equal(expectedEmail, actual);
    }

    [Fact]
    public void GivenSetupForCurrentCustomerEmail_WhenSetting_ThenSetupSetCanBeUsed()
    {
        // Arrange
        const string email = "customer@example.com";
        
        var callbackExecuted = false;
        
        _sut.SetupSet(x => x.CurrentCustomerEmail)
            .Callback(() => callbackExecuted = true);

        // Act
        _sut.Object.CurrentCustomerEmail = email;

        // Assert
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void GivenSetupForCurrentCustomerEmailWithCallback_WhenGetting_ThenCallbackExecuted()
    {
        // Arrange
        const string expectedEmail = "customer@example.com";
        var callbackExecuted = false;
        
        _sut.Setup(x => x.CurrentCustomerEmail)
            .Callback(() => callbackExecuted = true)
            .Returns(expectedEmail);

        // Act
        var actual = _sut.Object.CurrentCustomerEmail;

        // Assert
        Assert.True(callbackExecuted);
        Assert.Equal(expectedEmail, actual);
    }

    [Fact]
    public void GivenSetupForCurrentCustomerEmailWithThrows_WhenGetting_ThenThrowsException()
    {
        // Arrange
        _sut.Setup(x => x.CurrentCustomerEmail)
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.CurrentCustomerEmail);
    }

    [Fact]
    public void GivenSetupForCurrentCustomerEmailWithThrowsSpecificException_WhenGetting_ThenThrowsSpecificException()
    {
        // Arrange
        const string exceptionMessage = "Email service unavailable";
        
        _sut.Setup(x => x.CurrentCustomerEmail)
            .Throws(new InvalidOperationException(exceptionMessage));

        // Act & Assert
        var actual = Assert.Throws<InvalidOperationException>(() => _sut.Object.CurrentCustomerEmail);
        Assert.Equal("Email service unavailable", actual.Message);
    }

    [Fact]
    public void GivenSetupForMaxProcessingTimeoutSeconds_WhenGetting_ThenReturnsCorrectValue()
    {
        // Arrange
        const int expectedTimeout = 300;
        
        _sut.Setup(x => x.MaxProcessingTimeoutSeconds)
            .Returns(expectedTimeout);

        // Act
        var actual = _sut.Object.MaxProcessingTimeoutSeconds;

        // Assert
        Assert.Equal(expectedTimeout, actual);
    }

    [Fact]
    public void GivenSetupForMaxProcessingTimeoutSeconds_WhenSetting_ThenSetupSetCanBeUsed()
    {
        // Arrange
        const int timeout = 300;
        
        var callbackExecuted = false;
        
        _sut.SetupSet(x => x.MaxProcessingTimeoutSeconds)
            .Callback(() => callbackExecuted = true);

        // Act
        _sut.Object.MaxProcessingTimeoutSeconds = timeout;

        // Assert
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void GivenSetupForMaxProcessingTimeoutSecondsWithCallback_WhenGetting_ThenCallbackExecuted()
    {
        // Arrange
        const int expectedTimeout = 300;
        var callbackExecuted = false;
        
        _sut.Setup(x => x.MaxProcessingTimeoutSeconds)
            .Callback(() => callbackExecuted = true)
            .Returns(expectedTimeout);

        // Act
        var actual = _sut.Object.MaxProcessingTimeoutSeconds;

        // Assert
        Assert.True(callbackExecuted);
        Assert.Equal(expectedTimeout, actual);
    }

    [Fact]
    public void GivenSetupForIsValidationEnabled_WhenGetting_ThenReturnsCorrectValue()
    {
        // Arrange
        const bool expectedValue = true;
        
        _sut.Setup(x => x.IsValidationEnabled)
            .Returns(expectedValue);

        // Act
        var actual = _sut.Object.IsValidationEnabled;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupForIsValidationEnabled_WhenSetting_ThenSetupSetCanBeUsed()
    {
        // Arrange
        const bool validationEnabled = true;
        
        var callbackExecuted = false;
        
        _sut.SetupSet(x => x.IsValidationEnabled)
            .Callback(() => callbackExecuted = true);

        // Act
        _sut.Object.IsValidationEnabled = validationEnabled;

        // Assert
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void GivenSetupForIsValidationEnabledWithCallback_WhenGetting_ThenCallbackExecuted()
    {
        // Arrange
        const bool expectedValue = true;
        var callbackExecuted = false;
        
        _sut.Setup(x => x.IsValidationEnabled)
            .Callback(() => callbackExecuted = true)
            .Returns(expectedValue);

        // Act
        var actual = _sut.Object.IsValidationEnabled;

        // Assert
        Assert.True(callbackExecuted);
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupForDefaultShippingMethod_WhenGetting_ThenReturnsCorrectValue()
    {
        // Arrange
        const string expectedMethod = "Standard";
        
        _sut.Setup(x => x.DefaultShippingMethod)
            .Returns(expectedMethod);

        // Act
        var actual = _sut.Object.DefaultShippingMethod;

        // Assert
        Assert.Equal(expectedMethod, actual);
    }

    [Fact]
    public void GivenSetupForDefaultShippingMethod_WhenSetting_ThenSetupSetCanBeUsed()
    {
        // Arrange
        const string shippingMethod = "Express";
        
        var callbackExecuted = false;
        
        _sut.SetupSet(x => x.DefaultShippingMethod)
            .Callback(() => callbackExecuted = true);

        // Act
        _sut.Object.DefaultShippingMethod = shippingMethod;

        // Assert
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void GivenSetupForDefaultShippingMethodWithCallback_WhenGetting_ThenCallbackExecuted()
    {
        // Arrange
        const string expectedMethod = "Standard";
        var callbackExecuted = false;
        
        _sut.Setup(x => x.DefaultShippingMethod)
            .Callback(() => callbackExecuted = true)
            .Returns(expectedMethod);

        // Act
        var actual = _sut.Object.DefaultShippingMethod;

        // Assert
        Assert.True(callbackExecuted);
        Assert.Equal(expectedMethod, actual);
    }

    [Fact]
    public void GivenSetupForCurrentTaxRate_WhenGetting_ThenReturnsCorrectValue()
    {
        // Arrange
        const decimal expectedTaxRate = 0.0825m;
        
        _sut.Setup(x => x.CurrentTaxRate)
            .Returns(expectedTaxRate);

        // Act
        var actual = _sut.Object.CurrentTaxRate;

        // Assert
        Assert.Equal(expectedTaxRate, actual);
    }

    [Fact]
    public void GivenSetupForCurrentTaxRateWithCallback_WhenGetting_ThenCallbackExecuted()
    {
        // Arrange
        const decimal expectedTaxRate = 0.0825m;
        var callbackExecuted = false;
        
        _sut.Setup(x => x.CurrentTaxRate)
            .Callback(() => callbackExecuted = true)
            .Returns(expectedTaxRate);

        // Act
        var actual = _sut.Object.CurrentTaxRate;

        // Assert
        Assert.True(callbackExecuted);
        Assert.Equal(expectedTaxRate, actual);
    }

    [Fact]
    public void GivenSetupForCurrentTaxRateWithThrows_WhenGetting_ThenThrowsException()
    {
        // Arrange
        _sut.Setup(x => x.CurrentTaxRate)
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.CurrentTaxRate);
    }

    [Fact]
    public void GivenSetupForIsServiceOnline_WhenGetting_ThenReturnsCorrectValue()
    {
        // Arrange
        const bool expectedValue = true;
        
        _sut.Setup(x => x.IsServiceOnline)
            .Returns(expectedValue);

        // Act
        var actual = _sut.Object.IsServiceOnline;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupForIsServiceOnlineWithCallback_WhenGetting_ThenCallbackExecuted()
    {
        // Arrange
        const bool expectedValue = true;
        var callbackExecuted = false;
        
        _sut.Setup(x => x.IsServiceOnline)
            .Callback(() => callbackExecuted = true)
            .Returns(expectedValue);

        // Act
        var actual = _sut.Object.IsServiceOnline;

        // Assert
        Assert.True(callbackExecuted);
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupSequenceForCurrentCustomerEmail_WhenGettingMultipleTimes_ThenReturnsSequenceValues()
    {
        // Arrange
        _sut.SetupSequence(x => x.CurrentCustomerEmail)
            .Returns("customer1@example.com")
            .Returns("customer2@example.com")
            .Returns("customer3@example.com");

        // Act
        var firstCall = _sut.Object.CurrentCustomerEmail;
        var secondCall = _sut.Object.CurrentCustomerEmail;
        var thirdCall = _sut.Object.CurrentCustomerEmail;

        // Assert
        Assert.Equal("customer1@example.com", firstCall);
        Assert.Equal("customer2@example.com", secondCall);
        Assert.Equal("customer3@example.com", thirdCall);
    }

    [Fact]
    public void GivenSetupSequenceForMaxProcessingTimeoutSeconds_WhenGettingWithThrows_ThenThrowsException()
    {
        // Arrange
        _sut.SetupSequence(x => x.MaxProcessingTimeoutSeconds)
            .Returns(300)
            .Throws<InvalidOperationException>();

        // Act
        var firstCall = _sut.Object.MaxProcessingTimeoutSeconds;
        
        // Assert
        Assert.Equal(300, firstCall);
        Assert.Throws<InvalidOperationException>(() => _sut.Object.MaxProcessingTimeoutSeconds);
    }

    [Fact]
    public void GivenSetupSequenceForIsValidationEnabledWithCallback_WhenGetting_ThenCallbackExecuted()
    {
        // Arrange
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.IsValidationEnabled)
            .Returns(true)
            .Callback(() => callbackExecuted = true)
            .Returns(false);

        // Act
        var firstCall = _sut.Object.IsValidationEnabled;
        var secondCall = _sut.Object.IsValidationEnabled;

        // Assert
        Assert.True(firstCall);
        Assert.True(callbackExecuted);
        Assert.False(secondCall);
    }

    [Fact]
    public void GivenMixedSequenceSetupForDefaultShippingMethod_WhenGetting_ThenExecutesSequence()
    {
        // Arrange
        var callbackExecuted = false;
        
        _sut.SetupSequence(x => x.DefaultShippingMethod)
            .Returns("Standard")
            .Callback(() => callbackExecuted = true)
            .Returns("Express")
            .Throws<InvalidOperationException>();

        // Act
        var firstCall = _sut.Object.DefaultShippingMethod;
        var secondCall = _sut.Object.DefaultShippingMethod;

        // Assert
        Assert.Equal("Standard", firstCall);
        Assert.True(callbackExecuted);
        Assert.Equal("Express", secondCall);
        Assert.Throws<InvalidOperationException>(() => _sut.Object.DefaultShippingMethod);
    }

    [Fact]
    public void GivenNoSetup_WhenGettingCurrentCustomerEmail_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.CurrentCustomerEmail;

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenGettingMaxProcessingTimeoutSeconds_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.MaxProcessingTimeoutSeconds;

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    public void GivenNoSetup_WhenGettingIsValidationEnabled_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.IsValidationEnabled;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenGettingDefaultShippingMethod_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.DefaultShippingMethod;

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenGettingCurrentTaxRate_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.CurrentTaxRate;

        // Assert
        Assert.Equal(0m, actual);
    }

    [Fact]
    public void GivenNoSetup_WhenGettingIsServiceOnline_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.IsServiceOnline;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void GivenCurrentCustomerEmailAccessed_WhenVerifyingOnce_ThenVerificationSucceeds()
    {
        // Arrange
        _sut.Setup(x => x.CurrentCustomerEmail)
            .Returns("customer@example.com");

        // Act
        var _ = _sut.Object.CurrentCustomerEmail;

        // Assert
        _sut.Verify(x => x.CurrentCustomerEmail, Times.Once);
    }

    [Fact]
    public void GivenMaxProcessingTimeoutSecondsNotAccessed_WhenVerifyingNever_ThenVerificationSucceeds()
    {
        // Arrange
        // No access made

        // Act
        // No action

        // Assert
        _sut.Verify(x => x.MaxProcessingTimeoutSeconds, Times.Never);
    }

    [Fact]
    public void GivenIsValidationEnabledAccessedTwice_WhenVerifyingExactly_ThenVerificationSucceeds()
    {
        // Arrange
        _sut.Setup(x => x.IsValidationEnabled)
            .Returns(true);

        // Act
        var _ = _sut.Object.IsValidationEnabled;
        var __ = _sut.Object.IsValidationEnabled;

        // Assert
        _sut.Verify(x => x.IsValidationEnabled, Times.Exactly(2));
    }

    [Fact]
    public void GivenDefaultShippingMethodAccessedMultipleTimes_WhenVerifyingAtLeast_ThenVerificationSucceeds()
    {
        // Arrange
        _sut.Setup(x => x.DefaultShippingMethod)
            .Returns("Standard");

        // Act
        var _ = _sut.Object.DefaultShippingMethod;
        var __ = _sut.Object.DefaultShippingMethod;
        var ___ = _sut.Object.DefaultShippingMethod;

        // Assert
        _sut.Verify(x => x.DefaultShippingMethod, Times.AtLeast(2));
    }

    [Fact]
    public void GivenCurrentTaxRateAccessedOnce_WhenVerifyingAtMost_ThenVerificationSucceeds()
    {
        // Arrange
        _sut.Setup(x => x.CurrentTaxRate)
            .Returns(0.0825m);

        // Act
        var _ = _sut.Object.CurrentTaxRate;

        // Assert
        _sut.Verify(x => x.CurrentTaxRate, Times.AtMost(2));
    }

    [Fact]
    public void GivenIsServiceOnlineAccessedTwice_WhenVerifyingBetween_ThenVerificationSucceeds()
    {
        // Arrange
        _sut.Setup(x => x.IsServiceOnline)
            .Returns(true);

        // Act
        var _ = _sut.Object.IsServiceOnline;
        var __ = _sut.Object.IsServiceOnline;

        // Assert
        _sut.Verify(x => x.IsServiceOnline, Times.Between(1, 3));
    }

    [Fact]
    public void GivenPropertySetupWithComplexBehavior_WhenAccessingProperties_ThenBehavesCorrectly()
    {
        // Arrange
        var emailCallbackExecuted = false;
        var timeoutCallbackExecuted = false;
        var setterCallbackExecuted = false;
        
        _sut.Setup(x => x.CurrentCustomerEmail)
            .Callback(() => emailCallbackExecuted = true)
            .Returns("complex@example.com");
            
        _sut.Setup(x => x.MaxProcessingTimeoutSeconds)
            .Callback(() => timeoutCallbackExecuted = true)
            .Returns(600);
            
        _sut.SetupSet(x => x.IsValidationEnabled)
            .Callback(() => setterCallbackExecuted = true);

        // Act
        var email = _sut.Object.CurrentCustomerEmail;
        var timeout = _sut.Object.MaxProcessingTimeoutSeconds;
        
        _sut.Object.IsValidationEnabled = false;

        // Assert
        Assert.True(emailCallbackExecuted);
        Assert.True(timeoutCallbackExecuted);
        Assert.True(setterCallbackExecuted);
        Assert.Equal("complex@example.com", email);
        Assert.Equal(600, timeout);
    }
}