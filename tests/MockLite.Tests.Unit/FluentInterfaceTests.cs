using MockLite.Tests.Unit.Samples;

namespace MockLite.Tests.Unit;

public class FluentInterfaceTests
{
    [Fact]
    public void Setup_WhenChaining_ThenAllowsCallbackBeforeReturns()
    {
        // Arrange
        var callbackExecuted = false;
        
        var mock = new Mock<IService>();

        // Act
        mock.Setup(x => x.GetCount())
            .Callback(() => callbackExecuted = true)
            .Returns(42);

        var result = mock.Object.GetCount();

        // Assert
        Assert.Equal(42, result);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public async Task SetupAsync_WhenChaining_ThenAllowsCallbackBeforeReturnsAsync()
    {
        // Arrange
        var callbackExecuted = false;

        var mock = new Mock<IService>();

        // Act
        mock.Setup(x => x.GetDataAsync(1))
            .Callback(() => callbackExecuted = true)
            .ReturnsAsync("data");

        var result = await mock.Object.GetDataAsync(1);

        // Assert
        Assert.Equal("data", result);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void SetupGet_WhenChaining_ThenAllowsCallbackBeforeReturns()
    {
        // Arrange
        var callbackExecuted = false;

        var mock = new Mock<IOptions>();

        // Act
        mock.SetupGet(x => x.ConnectionString)
            .Callback(() => callbackExecuted = true)
            .Returns("connection");

        var result = mock.Object.ConnectionString;

        // Assert
        Assert.Equal("connection", result);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void SetupSet_WhenChaining_ThenAllowsCallbackBeforeThrows()
    {
        // Arrange
        var callbackExecuted = false;

        var mock = new Mock<IOptions>();

        mock.SetupSet(x => x.MaxRetries)
            .Callback(() => callbackExecuted = true)
            .Throws<ArgumentException>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => mock.Object.MaxRetries = 5);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void MethodCallback_WhenChainedWithReturns_ThenBothExecute()
    {
        // Arrange
        var callbackExecuted = false;
        var mock = new Mock<IService>();

        mock.Setup(x => x.GetCount())
            .Callback(() => callbackExecuted = true)
            .Returns(100);

        // Act
        var actual = mock.Object.GetCount();

        // Assert
        Assert.Equal(100, actual);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void PropertyGetterChaining_WhenUsingFluentInterface_ThenChainsCorrectly()
    {
        // Arrange
        var callbackExecuted = false;

        var mock = new Mock<IOptions>();

        mock.SetupGet(x => x.ConnectionString)
            .Callback(() => callbackExecuted = true)
            .Returns("test-connection");

        // Act
        var result = mock.Object.ConnectionString;

        // Assert
        Assert.Equal("test-connection", result);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void PropertySetterChaining_WhenUsingFluentInterface_ThenChainsCorrectly()
    {
        // Arrange
        var callbackExecuted = false;

        var mock = new Mock<IOptions>();

        mock.SetupSet(x => x.IsEnabled)
            .Callback(() => callbackExecuted = true)
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mock.Object.IsEnabled = true);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void SequenceChaining_WhenUsingFluentInterface_ThenChainsCorrectly()
    {
        // Arrange
        var executionOrder = new List<string>();

        var mock = new Mock<IService>();

        mock.SetupSequence(x => x.GetCount())
            .Callback(() => executionOrder.Add("first"))
            .Returns(1)
            .Callback(() => executionOrder.Add("second"))
            .Returns(2)
            .Callback(() => executionOrder.Add("third"))
            .Throws<InvalidOperationException>();

        // Act
        var result1 = mock.Object.GetCount();
        var result2 = mock.Object.GetCount();

        // Assert
        Assert.Equal(1, result1);
        Assert.Equal(2, result2);
        Assert.Throws<InvalidOperationException>(() => mock.Object.GetCount());
        Assert.Equal(["first", "second", "third"], executionOrder);
    }
}