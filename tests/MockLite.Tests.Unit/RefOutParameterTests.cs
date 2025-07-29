// ReSharper disable MemberCanBePrivate.Global
namespace MockLite.Tests.Unit;

public class RefOutParameterTests
{
    public interface IBasicRefOutMethods
    {
        bool TryGetValue(string key, out string value);
        
        void ModifyValue(ref int value);
    }

    private readonly Mock<IBasicRefOutMethods> _sut = new();

    [Fact]
    public void GivenMockWithRefOutMethods_WhenCreatingProxy_ThenProxyCreationSucceeds()
    {
        // Arrange & Act
        var proxy = _sut.Object;

        // Assert
        Assert.NotNull(proxy);
        Assert.IsAssignableFrom<IBasicRefOutMethods>(proxy);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingMethodWithOutParameter_ThenReturnsDefaultValues()
    {
        // Arrange & Act
        var actual = _sut.Object.TryGetValue("key", out var value);

        // Assert
        Assert.False(actual);
        Assert.Null(value);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingMethodWithRefParameter_ThenDoesNotModifyValue()
    {
        // Arrange
        const int originalValue = 42;
        
        var value = originalValue;

        // Act
        _sut.Object.ModifyValue(ref value);

        // Assert
        Assert.Equal(originalValue, value);
    }

    [Fact]
    public void GivenMultipleCalls_WhenCallingRefOutMethods_ThenEachCallReturnsDefaultValues()
    {
        // Arrange
        var refValue1 = 10;
        var refValue2 = 20;

        // Act
        _sut.Object.ModifyValue(ref refValue1);
        _sut.Object.ModifyValue(ref refValue2);
        
        var result1 = _sut.Object.TryGetValue("key1", out var outValue1);
        var result2 = _sut.Object.TryGetValue("key2", out var outValue2);

        // Assert
        Assert.Equal(10, refValue1);
        Assert.Equal(20, refValue2);
        Assert.False(result1);
        Assert.False(result2);
        Assert.Null(outValue1);
        Assert.Null(outValue2);
    }

    // Note: MockLite currently has limited ref/out parameter support.
    // Advanced scenarios like setting up ref/out parameter values are not yet implemented.
    // The framework can create proxies with ref/out methods but only returns default values.
}