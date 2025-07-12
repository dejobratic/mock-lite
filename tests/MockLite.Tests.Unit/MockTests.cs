using MockLite.Tests.Unit.Sample.Interfaces;

// ReSharper disable NotResolvedInText

namespace MockLite.Tests.Unit;

public class MockTests
{
    [Fact]
    public void BasicMockCreation_WhenCreatingMock_ThenSucceeds()
    {
        // Arrange & Act
        var mock = new Mock<IProductRepository>();

        // Assert
        Assert.NotNull(mock);
        Assert.NotNull(mock.Object);
    }

    [Fact]
    public void BasicSetupAndReturns_WhenCallingMethod_ThenReturnsSetupValue()
    {
        // Arrange
        const int expectedCount = 100;
        
        var mock = new Mock<IProductRepository>();
        
        mock.Setup(x => x.GetInStockCount())
            .Returns(expectedCount);

        // Act
        var actual = mock.Object.GetInStockCount();

        // Assert
        Assert.Equal(expectedCount, actual);
    }

    [Fact]
    public void PropertySetup_WhenAccessingProperty_ThenReturnsSetupValue()
    {
        // Arrange
        const int expectedActiveCount = 250;
        
        var mock = new Mock<ICustomerRepository>();
        
        mock.SetupGet(x => x.ActiveCustomersCount)
            .Returns(expectedActiveCount);

        // Act
        var actual = mock.Object.ActiveCustomersCount;

        // Assert
        Assert.Equal(expectedActiveCount, actual);
    }

    [Fact]
    public void SetupSequence_WhenCallingMethodMultipleTimes_ThenReturnsValuesInSequence()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        
        mock.SetupSequence(x => x.GetInStockCount())
            .Returns(10)
            .Returns(20)
            .Returns(30);

        // Act
        var result1 = mock.Object.GetInStockCount();
        var result2 = mock.Object.GetInStockCount();
        var result3 = mock.Object.GetInStockCount();
        var result4 = mock.Object.GetInStockCount(); // Should return default

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(20, result2);
        Assert.Equal(30, result3);
        Assert.Equal(0, result4); // Default value
    }

    [Fact]
    public void Throws_WhenCallingMethod_ThenThrowsException()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        
        mock.Setup(x => x.GetInStockCount())
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mock.Object.GetInStockCount());
    }

    [Fact]
    public void ThrowsWithSpecificException_WhenCallingMethod_ThenThrowsSpecificException()
    {
        // Arrange
        var specificException = new ArgumentException("Invalid product ID", "productId");
        
        var mock = new Mock<IProductRepository>();
        
        mock.Setup(x => x.GetInStockCount())
            .Throws(specificException);

        // Act & Assert
        var thrownException = Assert.Throws<ArgumentException>(() => mock.Object.GetInStockCount());
        Assert.Equal(specificException.Message, thrownException.Message);
        Assert.Equal(specificException.ParamName, thrownException.ParamName);
    }

    [Fact]
    public void Callback_WhenCallingMethod_ThenExecutesCallback()
    {
        // Arrange
        var callbackExecuted = false;
        
        var mock = new Mock<IProductRepository>();
        
        mock.Setup(x => x.GetInStockCount())
            .Callback(() => callbackExecuted = true)
            .Returns(42);

        // Act
        var actual = mock.Object.GetInStockCount();

        // Assert
        Assert.Equal(42, actual);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void Verify_WhenMethodCalled_ThenVerificationSucceeds()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();

        // Act
        mock.Object.GetInStockCount();
        mock.Object.GetInStockCount();

        // Assert
        mock.Verify(x => x.GetInStockCount(), Times.Exactly(2));
    }

    [Fact]
    public void VerifyNever_WhenMethodNotCalled_ThenVerificationSucceeds()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();

        // Act & Assert
        mock.Verify(x => x.GetInStockCount(), Times.Never);
    }

    [Fact]
    public void VerifyTimes_WhenMethodCalledSpecificTimes_ThenVerificationSucceeds()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();

        // Act
        mock.Object.GetInStockCount();
        mock.Object.GetInStockCount();
        mock.Object.GetInStockCount();

        // Assert
        mock.Verify(x => x.GetInStockCount(), Times.Exactly(3));
        mock.Verify(x => x.GetInStockCount(), Times.AtLeast(2));
        mock.Verify(x => x.GetInStockCount(), Times.AtMost(5));
        mock.Verify(x => x.GetInStockCount(), Times.Between(2, 4));
    }

    [Fact]
    public void VerifyFailure_WhenMethodNotCalledEnough_ThenThrowsMockException()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        
        // Act
        mock.Object.GetInStockCount(); // Called once

        // Assert
        var exception = Assert.Throws<MockException>(() =>
            mock.Verify(x => x.GetInStockCount(), Times.Exactly(3)));
        
        Assert.Contains("Expected 3-3 calls, but got 1", exception.Message);
    }

    [Fact]
    public void SequenceWithThrows_WhenCallingMethodInSequence_ThenExecutesSequenceWithException()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        
        mock.SetupSequence(x => x.GetInStockCount())
            .Returns(10)
            .Returns(20)
            .Throws<InvalidOperationException>();

        // Act & Assert
        var result1 = mock.Object.GetInStockCount();
        Assert.Equal(10, result1);

        var result2 = mock.Object.GetInStockCount();
        Assert.Equal(20, result2);

        Assert.Throws<InvalidOperationException>(() => mock.Object.GetInStockCount());
    }

    [Fact]
    public void MultipleSetupsOnSameMock_WhenCallingDifferentMethods_ThenEachReturnsCorrectValue()
    {
        // Arrange
        const int expectedCount = 50;
        const decimal expectedPrice = 25.99m;
        
        var mock = new Mock<IProductRepository>();
        
        mock.Setup(x => x.GetInStockCount())
            .Returns(expectedCount);
        
        mock.Setup(x => x.GetAveragePrice())
            .Returns(expectedPrice);

        // Act
        var actualCount = mock.Object.GetInStockCount();
        var actualPrice = mock.Object.GetAveragePrice();

        // Assert
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedPrice, actualPrice);
    }

    [Fact]
    public void DefaultBehavior_WhenMethodNotSetup_ThenReturnsDefaultValue()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();

        // Act
        var count = mock.Object.GetInStockCount();
        var price = mock.Object.GetAveragePrice();

        // Assert
        Assert.Equal(0, count);
        Assert.Equal(0m, price);
    }

    [Fact]
    public void SequenceWithCallback_WhenCallingMethod_ThenExecutesCallbacks()
    {
        // Arrange
        var executionLog = new List<string>();
        
        var mock = new Mock<IProductRepository>();
        
        mock.SetupSequence(x => x.GetInStockCount())
            .Returns(10)
            .Callback(() => executionLog.Add("First call"))
            .Returns(20)
            .Callback(() => executionLog.Add("Second call"));

        // Act
        var result1 = mock.Object.GetInStockCount();
        var result2 = mock.Object.GetInStockCount();

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(20, result2);
        Assert.Equal(2, executionLog.Count);
        Assert.Equal("First call", executionLog[0]);
        Assert.Equal("Second call", executionLog[1]);
    }

    [Fact]
    public void OverriddenSetup_WhenSetupOverridden_ThenUsesLatestSetup()
    {
        // Arrange
        const int firstValue = 10;
        const int secondValue = 20;
        
        var mock = new Mock<IProductRepository>();
        
        mock.Setup(x => x.GetInStockCount())
            .Returns(firstValue);
        
        mock.Setup(x => x.GetInStockCount())
            .Returns(secondValue);

        // Act
        var actual = mock.Object.GetInStockCount();

        // Assert
        Assert.Equal(secondValue, actual);
    }

    [Fact]
    public void CallbackWithParameters_WhenCallingMethodWithArgs_ThenCallbackReceivesArgs()
    {
        // Arrange
        var capturedEmail = "";
        const string email = "test@example.com";
        
        var mock = new Mock<ICustomerRepository>();
        
        mock.Setup(x => x.IsEmailUnique(email))
            .Callback<string>(e => capturedEmail = e)
            .Returns(true);

        // Act
        var result = mock.Object.IsEmailUnique(email);

        // Assert
        Assert.True(result);
        Assert.Equal(email, capturedEmail);
    }

    [Fact]
    public void ComplexParameterCallback_WhenMethodHasMultipleParameters_ThenCallbackReceivesAll()
    {
        // Arrange
        var capturedAmount = 0m;
        const decimal amount = 100.50m;
        
        var mock = new Mock<IPaymentService>();
        
        mock.Setup(x => x.GetProcessingFee(amount))
            .Callback<decimal>(amt => capturedAmount = amt)
            .Returns(2.50m);

        // Act
        var actual = mock.Object.GetProcessingFee(amount);

        // Assert
        Assert.Equal(2.50m, actual);
        Assert.Equal(amount, capturedAmount);
    }

    [Fact]
    public void MockUsageDemo_WhenUsingMultipleMockFeatures_ThenAllFeaturesWork()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        
        // Setup method returns
        productRepositoryMock
            .Setup(x => x.GetInStockCount())
            .Returns(100);
        
        productRepositoryMock
            .Setup(x => x.GetAveragePrice())
            .Returns(25.99m);
        
        // Setup sequence
        productRepositoryMock
            .SetupSequence(x => x.GetInStockCount())
            .Returns(100)
            .Returns(90)
            .Returns(80);
        
        // Setup property
        customerRepositoryMock
            .SetupGet(x => x.ActiveCustomersCount)
            .Returns(500);
        
        // Setup with callback
        var capturedEmail = "";
        customerRepositoryMock
            .Setup(x => x.IsEmailUnique("test@example.com"))
            .Callback<string>(email => capturedEmail = email)
            .Returns(true);

        // Act
        var stockCount1 = productRepositoryMock.Object.GetInStockCount(); // 100 (from the sequence)
        var stockCount2 = productRepositoryMock.Object.GetInStockCount(); // 90 (from the sequence)
        var averagePrice = productRepositoryMock.Object.GetAveragePrice();
        var activeCustomers = customerRepositoryMock.Object.ActiveCustomersCount;
        var isEmailUnique = customerRepositoryMock.Object.IsEmailUnique("test@example.com");

        // Assert
        Assert.Equal(100, stockCount1);
        Assert.Equal(90, stockCount2);
        Assert.Equal(25.99m, averagePrice);
        Assert.Equal(500, activeCustomers);
        Assert.True(isEmailUnique);
        Assert.Equal("test@example.com", capturedEmail);
        
        // Verify calls
        productRepositoryMock.Verify(x => x.GetInStockCount(), Times.Exactly(2));
        productRepositoryMock.Verify(x => x.GetAveragePrice(), Times.Once);
        customerRepositoryMock.Verify(x => x.IsEmailUnique("test@example.com"), Times.Once);
    }
}