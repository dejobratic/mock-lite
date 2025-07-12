// ReSharper disable NotResolvedInText

// ReSharper disable MethodHasAsyncOverload
namespace MockLite.Tests.Unit;

// Simple test interfaces for async testing
public interface ISimpleRepository
{
    int GetCount();
    decimal GetAveragePrice();
    Task<string> GetDataAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task SaveAsync(string data);
    bool IsValid(string input);
}

public class SimpleData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class MockTests
{
    [Fact]
    public void BasicMockCreation_WhenCreatingMock_ThenSucceeds()
    {
        // Arrange & Act
        var mock = new Mock<ISimpleRepository>();

        // Assert
        Assert.NotNull(mock);
        Assert.NotNull(mock.Object);
    }

    [Fact]
    public void BasicSetupAndReturns_WhenCallingMethod_ThenReturnsSetupValue()
    {
        // Arrange
        const int expectedCount = 100;
        
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.GetCount())
            .Returns(expectedCount);

        // Act
        var actual = mock.Object.GetCount();

        // Assert
        Assert.Equal(expectedCount, actual);
    }

    [Fact]
    public void SetupSequence_WhenCallingMethodMultipleTimes_ThenReturnsValuesInSequence()
    {
        // Arrange
        var mock = new Mock<ISimpleRepository>();
        
        mock.SetupSequence(x => x.GetCount())
            .Returns(10)
            .Returns(20)
            .Returns(30);

        // Act
        var result1 = mock.Object.GetCount();
        var result2 = mock.Object.GetCount();
        var result3 = mock.Object.GetCount();
        var result4 = mock.Object.GetCount(); // Should return default

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
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.GetCount())
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mock.Object.GetCount());
    }

    [Fact]
    public void Callback_WhenCallingMethod_ThenExecutesCallback()
    {
        // Arrange
        var callbackExecuted = false;
        
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.GetCount())
            .Callback(() => callbackExecuted = true)
            .Returns(42);

        // Act
        var actual = mock.Object.GetCount();

        // Assert
        Assert.Equal(42, actual);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void Verify_WhenMethodCalled_ThenVerificationSucceeds()
    {
        // Arrange
        var mock = new Mock<ISimpleRepository>();

        // Act
        mock.Object.GetCount();
        mock.Object.GetCount();

        // Assert
        mock.Verify(x => x.GetCount(), Times.Exactly(2));
    }

    [Fact]
    public async Task AsyncMethodSetup_WhenUsingReturnsWithTask_ThenReturnsAsyncValue()
    {
        // Arrange
        const string expectedData = "Test Data";
        
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.GetDataAsync(1))
            .Returns(Task.FromResult(expectedData));

        // Act
        var result = await mock.Object.GetDataAsync(1);

        // Assert
        Assert.Equal(expectedData, result);
    }
    
    [Fact]
    public async Task AsyncMethodSetup_WhenUsingReturnsAsync_ThenReturnsAsyncValue()
    {
        // Arrange
        const string expectedData = "Test Data";
        
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.GetDataAsync(1))
            .ReturnsAsync(expectedData);

        // Act
        var actual = await mock.Object.GetDataAsync(1);

        // Assert
        Assert.Equal(expectedData, actual);
    }

    [Fact]
    public async Task AsyncMethodSetup_WhenUsingThrows_ThenThrowsAsyncException()
    {
        // Arrange
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.GetDataAsync(999))
            .Throws<ArgumentException>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => mock.Object.GetDataAsync(999));
    }
    
    [Fact]
    public async Task AsyncMethodSetup_WhenUsingThrowsAsync_ThenThrowsAsyncException()
    {
        // Arrange
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.GetDataAsync(999))
            .ThrowsAsync<ArgumentException>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => mock.Object.GetDataAsync(999));
    }

    [Fact]
    public async Task AsyncVoidMethodSetup_WhenUsingThrows_ThenThrowsException()
    {
        // Arrange
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.SaveAsync("invalid-data"))
            .Throws<InvalidOperationException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            mock.Object.SaveAsync("invalid-data"));
    }

    [Fact]
    public async Task AsyncSequenceSetup_WhenCallingAsyncMethodMultipleTimes_ThenReturnsSequenceValues()
    {
        // Arrange
        var mock = new Mock<ISimpleRepository>();
        
        mock.SetupSequence(x => x.GetDataAsync(1))
            .Returns(Task.FromResult("Data 1"))
            .Returns(Task.FromResult("Data 2"))
            .Throws<ArgumentException>();

        // Act
        var result1 = await mock.Object.GetDataAsync(1);
        var result2 = await mock.Object.GetDataAsync(1);

        // Assert
        Assert.Equal("Data 1", result1);
        Assert.Equal("Data 2", result2);
        await Assert.ThrowsAsync<ArgumentException>(() => mock.Object.GetDataAsync(1));
    }


    [Fact]
    public async Task ReturnsAsyncExtension_WhenUsingImprovedSyntax_ThenWorksCorrectly()
    {
        // Arrange
        const string expectedData = "Extension Method Test";
        var mock = new Mock<ISimpleRepository>();
        
        mock.Setup(x => x.GetDataAsync(42))
            .ReturnsAsync(expectedData);

        // Act
        var actual = await mock.Object.GetDataAsync(42);

        // Assert
        Assert.Equal(expectedData, actual);
    }

    [Fact]
    public async Task DefaultBehavior_WhenAsyncMethodNotSetup_ThenReturnsDefaultValue()
    {
        // Arrange
        var mock = new Mock<ISimpleRepository>();

        // Act
        var count = mock.Object.GetCount();
        var price = mock.Object.GetAveragePrice();
        var data = await mock.Object.GetDataAsync(1);

        // Assert
        Assert.Equal(0, count);
        Assert.Equal(0m, price);
        Assert.Null(data);
    }
}