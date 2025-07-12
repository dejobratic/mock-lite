using MockLite.Tests.Unit.Samples;

namespace MockLite.Tests.Unit;

public class CallbackTests
{
    [Fact]
    public void Callback_WhenCallingMethod_ThenExecutesCallback()
    {
        // Arrange
        var callbackExecuted = false;
        
        var mock = new Mock<IService>();
        
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
    public void MethodCallback_WhenUsingParameterCallback_ThenCapturesArguments()
    {
        // Arrange
        var capturedInput = "";
        
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.IsValid("captured-input"))
            .Callback(args => capturedInput = (string)args[0])
            .Returns(true);

        // Act
        var result = mock.Object.IsValid("captured-input");

        // Assert
        Assert.True(result);
        Assert.Equal("captured-input", capturedInput);
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
        var result = mock.Object.GetCount();

        // Assert
        Assert.Equal(100, result);
        Assert.True(callbackExecuted);
    }

    [Fact]
    public void CallbackExceptions_WhenCallbackThrows_ThenPropagatesException()
    {
        // Arrange
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.GetCount())
            .Callback(() => throw new InvalidOperationException("Callback failed"))
            .Returns(100);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mock.Object.GetCount());
    }

    [Fact]
    public void MultipleCallbacks_WhenSetupHasBothCallbackTypes_ThenBothExecute()
    {
        // Arrange
        var simpleCallbackExecuted = false;
        string? capturedParameter = null;
        
        var mock = new Mock<IService>();
        
        // Note: In practice, you'd typically use one or the other, but testing both
        mock.Setup(x => x.IsValid("parameter-test"))
            .Callback(() => simpleCallbackExecuted = true)
            .Callback(args => capturedParameter = (string)args[0])
            .Returns(true);

        // Act
        var result = mock.Object.IsValid("parameter-test");

        // Assert
        Assert.True(result);
        Assert.True(simpleCallbackExecuted);
        Assert.Equal("parameter-test", capturedParameter);
    }
}