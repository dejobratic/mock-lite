// ReSharper disable MethodHasAsyncOverload

using MockLite.Tests.Unit.Samples;

namespace MockLite.Tests.Unit;

public class AsyncMethodTests
{
    [Fact]
    public async Task AsyncMethodSetup_WhenUsingReturnsWithTask_ThenReturnsAsyncValue()
    {
        // Arrange
        const string expectedData = "Test Data";
        
        var mock = new Mock<IService>();
        
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
        
        var mock = new Mock<IService>();
        
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
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.GetDataAsync(999))
            .Throws<ArgumentException>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => mock.Object.GetDataAsync(999));
    }
    
    [Fact]
    public async Task AsyncMethodSetup_WhenUsingThrowsAsync_ThenThrowsAsyncException()
    {
        // Arrange
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.GetDataAsync(999))
            .ThrowsAsync<ArgumentException>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => mock.Object.GetDataAsync(999));
    }

    [Fact]
    public async Task AsyncVoidMethodSetup_WhenUsingThrows_ThenThrowsException()
    {
        // Arrange
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.SaveAsync("invalid-data"))
            .Throws<InvalidOperationException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            mock.Object.SaveAsync("invalid-data"));
    }

    [Fact]
    public async Task ReturnsAsyncExtension_WhenUsingImprovedSyntax_ThenWorksCorrectly()
    {
        // Arrange
        const string expectedData = "Extension Method Test";
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.GetDataAsync(42))
            .ReturnsAsync(expectedData);

        // Act
        var actual = await mock.Object.GetDataAsync(42);

        // Assert
        Assert.Equal(expectedData, actual);
    }

    [Fact]
    public async Task AsyncMethodCallback_WhenUsingCallback_ThenExecutesBeforeReturning()
    {
        // Arrange
        var executionOrder = new List<string>();
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.GetDataAsync(1))
            .Callback(() => executionOrder.Add("callback"))
            .Returns(() =>
            {
                executionOrder.Add("returns");
                return Task.FromResult("data");
            });

        // Act
        var result = await mock.Object.GetDataAsync(1);

        // Assert
        Assert.Equal("data", result);
        Assert.Equal(["callback", "returns"], executionOrder);
    }

    [Fact]
    public void VoidMethodCallback_WhenUsingCallback_ThenExecutesCallback()
    {
        // Arrange
        var capturedData = "";
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.SaveAsync("saved-data"))
            .Callback(args => capturedData = (string)args[0])
            .Returns(Task.CompletedTask);

        // Act
        mock.Object.SaveAsync("saved-data");

        // Assert
        Assert.Equal("saved-data", capturedData);
    }
}