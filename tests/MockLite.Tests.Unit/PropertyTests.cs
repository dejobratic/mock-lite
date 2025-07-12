using MockLite.Tests.Unit.Samples;

namespace MockLite.Tests.Unit;

public class PropertyTests
{
    [Fact]
    public void SetupGet_WhenPropertyAccessed_ThenReturnsSetupValue()
    {
        // Arrange
        const string expectedConnectionString = "Data Source=localhost";
        
        var mock = new Mock<IOptions>();
        
        mock.SetupGet(x => x.ConnectionString)
            .Returns(expectedConnectionString);

        // Act
        var actual = mock.Object.ConnectionString;

        // Assert
        Assert.Equal(expectedConnectionString, actual);
    }

    [Fact]
    public void SetupGet_WhenUsingValueFunction_ThenReturnsComputedValue()
    {
        // Arrange
        var callCount = 0;
        
        var mock = new Mock<IOptions>();
        
        mock.SetupGet(x => x.MaxRetries)
            .Returns(() => ++callCount * 5);

        // Act
        var result1 = mock.Object.MaxRetries;
        var result2 = mock.Object.MaxRetries;

        // Assert
        Assert.Equal(5, result1);
        Assert.Equal(10, result2);
    }

    [Fact]
    public void SetupGet_WhenUsingThrows_ThenThrowsException()
    {
        // Arrange
        var mock = new Mock<IOptions>();
        
        mock.SetupGet(x => x.Timeout)
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mock.Object.Timeout);
    }

    [Fact]
    public void SetupGet_WhenUsingCallback_ThenExecutesCallback()
    {
        // Arrange
        var callbackExecuted = false;
        
        var mock = new Mock<IOptions>();
        
        mock.SetupGet(x => x.IsEnabled)
            .Callback(() => callbackExecuted = true)
            .Returns(true);

        // Act
        var result = mock.Object.IsEnabled;

        // Assert
        Assert.True(result);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void SetupSet_WhenPropertySet_ThenCallbackExecuted()
    {
        // Arrange
        var capturedValue = "";
        
        var mock = new Mock<IOptions>();
        
        mock.SetupSet(x => x.ConnectionString)
            .Callback(() => capturedValue = "callback-executed");

        // Act
        mock.Object.ConnectionString = "test-value";

        // Assert
        Assert.Equal("callback-executed", capturedValue);
    }

    [Fact]
    public void SetupSet_WhenUsingCallbackWithParameters_ThenCapturesSetValue()
    {
        // Arrange
        string? capturedValue = null;
        
        var mock = new Mock<IOptions>();
        
        mock.SetupSet(x => x.ConnectionString)
            .Callback(args => capturedValue = (string)args[0]);

        // Act
        mock.Object.ConnectionString = "actual-value";

        // Assert
        Assert.Equal("actual-value", capturedValue);
    }

    [Fact]
    public void SetupSet_WhenUsingThrows_ThenThrowsException()
    {
        // Arrange
        var mock = new Mock<IOptions>();
        
        mock.SetupSet(x => x.MaxRetries)
            .Throws<ArgumentException>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => mock.Object.MaxRetries = 5);
    }

    [Fact]
    public void SetupSet_WhenChaining_ThenAllowsCallbackBeforeThrows()
    {
        // Arrange
        var callbackExecuted = false;
        
        var mock = new Mock<IOptions>();

        // Act
        mock.SetupSet(x => x.IsEnabled)
            .Callback(() => callbackExecuted = true)
            .Throws<InvalidOperationException>();

        // Assert
        Assert.Throws<InvalidOperationException>(() => mock.Object.IsEnabled = true);
        Assert.True(callbackExecuted);
    }
}