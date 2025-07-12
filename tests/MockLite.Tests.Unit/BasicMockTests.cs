using MockLite.Tests.Unit.Samples;

namespace MockLite.Tests.Unit;

public class BasicMockTests
{
    [Fact]
    public void BasicMockCreation_WhenCreatingMock_ThenSucceeds()
    {
        // Arrange & Act
        var mock = new Mock<IService>();

        // Assert
        Assert.NotNull(mock);
        Assert.NotNull(mock.Object);
    }

    [Fact]
    public void BasicSetupAndReturns_WhenCallingMethod_ThenReturnsSetupValue()
    {
        // Arrange
        const int expectedCount = 100;
        
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.GetCount())
            .Returns(expectedCount);

        // Act
        var actual = mock.Object.GetCount();

        // Assert
        Assert.Equal(expectedCount, actual);
    }

    [Fact]
    public void Throws_WhenCallingMethod_ThenThrowsException()
    {
        // Arrange
        var mock = new Mock<IService>();
        
        mock.Setup(x => x.GetCount())
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => mock.Object.GetCount());
    }

    [Fact]
    public void Verify_WhenMethodCalled_ThenVerificationSucceeds()
    {
        // Arrange
        var mock = new Mock<IService>();

        // Act
        mock.Object.GetCount();
        mock.Object.GetCount();

        // Assert
        mock.Verify(x => x.GetCount(), Times.Exactly(2));
    }

    [Fact]
    public async Task DefaultBehavior_WhenAsyncMethodNotSetup_ThenReturnsDefaultValue()
    {
        // Arrange
        var mock = new Mock<IService>();

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