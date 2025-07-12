using MockLite.Tests.Unit.Samples;

namespace MockLite.Tests.Unit;

public class SequenceTests
{
    [Fact]
    public void SetupSequence_WhenCallingMethodMultipleTimes_ThenReturnsValuesInSequence()
    {
        // Arrange
        var mock = new Mock<IService>();
        
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
    public async Task AsyncSequenceSetup_WhenCallingAsyncMethodMultipleTimes_ThenReturnsSequenceValues()
    {
        // Arrange
        var mock = new Mock<IService>();
        
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
    public void SequenceCallback_WhenUsingCallbacks_ThenExecutesInOrder()
    {
        // Arrange
        var executionOrder = new List<int>();
        
        var mock = new Mock<IService>();
        
        mock.SetupSequence(x => x.GetCount())
            .Callback(() => executionOrder.Add(1))
            .Returns(10)
            .Callback(() => executionOrder.Add(2))
            .Returns(20)
            .Callback(() => executionOrder.Add(3))
            .Returns(30);

        // Act
        var result1 = mock.Object.GetCount();
        var result2 = mock.Object.GetCount();
        var result3 = mock.Object.GetCount();

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(20, result2);
        Assert.Equal(30, result3);
        Assert.Equal([1, 2, 3], executionOrder);
    }

    [Fact]
    public async Task SequenceCallbackWithParameters_WhenCalled_ThenCapturesParameters()
    {
        // Arrange
        var capturedValues = new List<string>();
        
        var mock = new Mock<IService>();

        mock.SetupSequence(x => x.GetDataAsync(1))
            .Callback(args => capturedValues.Add($"call1-{args[0]}"))
            .ReturnsAsync("data1")
            .Callback(args => capturedValues.Add($"call2-{args[0]}"))
            .ReturnsAsync("data2");

        // Act
        var result1 = await mock.Object.GetDataAsync(1);
        var result2 = await mock.Object.GetDataAsync(1);

        // Assert
        Assert.Equal("data1", result1);
        Assert.Equal("data2", result2);
        Assert.Equal(["call1-1", "call2-1"], capturedValues);
    }
}