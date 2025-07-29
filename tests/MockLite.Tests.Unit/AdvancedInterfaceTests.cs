// ReSharper disable MemberCanBePrivate.Global

using System.Diagnostics.CodeAnalysis;

namespace MockLite.Tests.Unit;

[SuppressMessage("Usage", "xUnit1031:Do not use blocking task operations in test method")]
public class AdvancedInterfaceTests
{
    public interface IAdvancedInterface
    {
        // Indexer property
        string this[int index] { get; set; }
        
        // Properties with different access levels
        string ReadOnlyProperty { get; }
        string WriteOnlyProperty { set; }
        string ReadWriteProperty { get; set; }
        
        // Generic methods with constraints
        T Process<T>(T item) where T : class;
        TResult Transform<TSource, TResult>(TSource source) where TSource : new() where TResult : class;
        
        // Methods with optional parameters
        void ProcessWithDefaults(string required, int optional = 42, bool flag = true);
        
        // Methods with a params array
        void ProcessMultiple(params string[] items);
        
        // Async methods
        Task<string> GetDataAsync();
        ValueTask<int> GetCountAsync();
        
        // Methods returning interfaces
        IEnumerable<string> GetItems();
        IQueryable<int> GetQueryableData();
    }

    private readonly Mock<IAdvancedInterface> _sut = new();

    [Fact]
    public void GivenMockWithAdvancedFeatures_WhenCreatingProxy_ThenProxyCreationSucceeds()
    {
        // Arrange & Act
        var proxy = _sut.Object;

        // Assert
        Assert.NotNull(proxy);
        Assert.IsAssignableFrom<IAdvancedInterface>(proxy);
    }

    [Fact]
    public void GivenNoSetup_WhenAccessingIndexer_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object[0];

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenSettingIndexer_ThenNoExceptionThrown()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => _sut.Object[0] = "test");
        Assert.Null(exception);
    }

    [Fact]
    public void GivenNoSetup_WhenAccessingReadOnlyProperty_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.ReadOnlyProperty;

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenSettingWriteOnlyProperty_ThenNoExceptionThrown()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => _sut.Object.WriteOnlyProperty = "test");
        Assert.Null(exception);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingGenericMethodWithConstraints_ThenReturnsDefault()
    {
        // Arrange
        const string input = "test";

        // Act
        var actual = _sut.Object.Process(input);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingGenericMethodWithMultipleConstraints_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.Transform<List<int>, string>([]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingMethodWithOptionalParameters_ThenNoExceptionThrown()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => _sut.Object.ProcessWithDefaults("required"));
        Assert.Null(exception);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingMethodWithParamsArray_ThenNoExceptionThrown()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => _sut.Object.ProcessMultiple("a", "b", "c"));
        Assert.Null(exception);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingAsyncMethod_ThenReturnsCompletedTask()
    {
        // Arrange & Act
        var actual = _sut.Object.GetDataAsync();

        // Assert
        Assert.NotNull(actual);
        Assert.True(actual.IsCompleted);
        Assert.Null(actual.Result);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingValueTaskMethod_ThenReturnsDefault()
    {
        // Arrange & Act
        var actual = _sut.Object.GetCountAsync();

        // Assert
        Assert.True(actual.IsCompleted);
        Assert.Equal(0, actual.Result);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingMethodReturningInterface_ThenReturnsNull()
    {
        // Arrange & Act
        var actual = _sut.Object.GetItems();

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GivenNoSetup_WhenCallingMethodReturningQueryable_ThenReturnsNull()
    {
        // Arrange & Act
        var actual = _sut.Object.GetQueryableData();

        // Assert
        Assert.Null(actual);
    }

    // Setup Tests with Returns
    [Fact]
    public void GivenSetupWithReturns_WhenCallingMethod_ThenReturnsConfiguredValue()
    {
        // Arrange
        var expectedValue = "configured result";
        _sut.Setup(x => x.Process("test")).Returns(expectedValue);

        // Act
        var actual = _sut.Object.Process("test");

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupWithReturnsFunc_WhenCallingMethod_ThenReturnsValueFromFunc()
    {
        // Arrange
        var expectedValue = "dynamic result";
        _sut.Setup(x => x.Process("test")).Returns(() => expectedValue);

        // Act
        var actual = _sut.Object.Process("test");

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupWithReturns_WhenCallingAsyncMethod_ThenReturnsConfiguredTask()
    {
        // Arrange
        var expectedValue = "async result";
        _sut.Setup(x => x.GetDataAsync()).Returns(Task.FromResult(expectedValue));

        // Act
        var actual = _sut.Object.GetDataAsync().Result;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupWithReturns_WhenCallingValueTaskMethod_ThenReturnsConfiguredValueTask()
    {
        // Arrange
        var expectedValue = 42;
        _sut.Setup(x => x.GetCountAsync()).Returns(new ValueTask<int>(expectedValue));

        // Act
        var actual = _sut.Object.GetCountAsync().Result;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    // Setup Tests with Throws
    [Fact]
    public void GivenSetupWithThrows_WhenCallingMethod_ThenThrowsConfiguredException()
    {
        // Arrange
        _sut.Setup(x => x.Process("test")).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.Process("test"));
    }

    [Fact]
    public void GivenSetupWithThrowsInstance_WhenCallingMethod_ThenThrowsConfiguredException()
    {
        // Arrange
        var expectedException = new ArgumentException("Custom message");
        _sut.Setup(x => x.Process("test")).Throws(expectedException);

        // Act & Assert
        var actual = Assert.Throws<ArgumentException>(() => _sut.Object.Process("test"));
        Assert.Equal("Custom message", actual.Message);
    }

    [Fact]
    public void GivenSetupWithThrowsForVoidMethod_WhenCallingMethod_ThenThrowsConfiguredException()
    {
        // Arrange
        _sut.Setup(x => x.ProcessWithDefaults("test", 42, true)).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.ProcessWithDefaults("test"));
        Assert.Throws<InvalidOperationException>(() => _sut.Object.ProcessWithDefaults("test", 42));
        Assert.Throws<InvalidOperationException>(() => _sut.Object.ProcessWithDefaults("test", 42, true));
    }

    // SetupGet Tests with Returns
    [Fact]
    public void GivenSetupGetWithReturns_WhenAccessingProperty_ThenReturnsConfiguredValue()
    {
        // Arrange
        var expectedValue = "configured property value";
        _sut.SetupGet(x => x.ReadOnlyProperty).Returns(expectedValue);

        // Act
        var actual = _sut.Object.ReadOnlyProperty;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupGetWithReturnsFunc_WhenAccessingProperty_ThenReturnsValueFromFunc()
    {
        // Arrange
        var expectedValue = "dynamic property value";
        _sut.SetupGet(x => x.ReadOnlyProperty).Returns(() => expectedValue);

        // Act
        var actual = _sut.Object.ReadOnlyProperty;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupGetWithReturns_WhenAccessingReadWriteProperty_ThenReturnsConfiguredValue()
    {
        // Arrange
        var expectedValue = "read-write property value";
        _sut.SetupGet(x => x.ReadWriteProperty).Returns(expectedValue);

        // Act
        var actual = _sut.Object.ReadWriteProperty;

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void GivenSetupGetWithReturns_WhenAccessingIndexer_ThenReturnsConfiguredValue()
    {
        // Arrange
        var expectedValue = "indexed value";
        _sut.SetupGet(x => x[0]).Returns(expectedValue);

        // Act
        var actual = _sut.Object[0];

        // Assert
        Assert.Equal(expectedValue, actual);
    }

    // SetupGet Tests with Throws
    [Fact]
    public void GivenSetupGetWithThrows_WhenAccessingProperty_ThenThrowsConfiguredException()
    {
        // Arrange
        _sut.SetupGet(x => x.ReadOnlyProperty).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.ReadOnlyProperty);
    }

    [Fact]
    public void GivenSetupGetWithThrowsInstance_WhenAccessingProperty_ThenThrowsConfiguredException()
    {
        // Arrange
        var expectedException = new ArgumentException("Property access error");
        _sut.SetupGet(x => x.ReadOnlyProperty).Throws(expectedException);

        // Act & Assert
        var actual = Assert.Throws<ArgumentException>(() => _sut.Object.ReadOnlyProperty);
        Assert.Equal("Property access error", actual.Message);
    }

    // SetupSet Tests with Throws (note: WriteOnlyProperty cannot be used in SetupSet as it lacks get accessor)

    [Fact]
    public void GivenSetupSetWithThrows_WhenSettingReadWriteProperty_ThenThrowsConfiguredException()
    {
        // Arrange
        _sut.SetupSet(x => x.ReadWriteProperty).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.ReadWriteProperty = "test");
    }

    [Fact]
    public void GivenSetupSetWithThrows_WhenSettingIndexer_ThenThrowsConfiguredException()
    {
        // Arrange
        _sut.SetupSet(x => x[0]).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object[0] = "test");
    }

    // SetupSequence Tests with Returns
    [Fact]
    public void GivenSetupSequenceWithReturns_WhenCallingMethodMultipleTimes_ThenReturnsSequentialValues()
    {
        // Arrange
        _sut.SetupSequence(x => x.Process("test"))
            .Returns("first")
            .Returns("second")
            .Returns("third");

        // Act
        var first = _sut.Object.Process("test");
        var second = _sut.Object.Process("test");
        var third = _sut.Object.Process("test");
        var fourth = _sut.Object.Process("test");

        // Assert
        Assert.Equal("first", first);
        Assert.Equal("second", second);
        Assert.Equal("third", third);
        Assert.Null(fourth); // Should return default after a sequence is exhausted
    }

    [Fact]
    public void GivenSetupSequenceWithReturnsFunc_WhenCallingMethodMultipleTimes_ThenReturnsSequentialValues()
    {
        // Arrange
        var counter = 0;
        _sut.SetupSequence(x => x.Process("test"))
            .Returns(() => $"value{++counter}")
            .Returns(() => $"value{++counter}");

        // Act
        var first = _sut.Object.Process("test");
        var second = _sut.Object.Process("test");

        // Assert
        Assert.Equal("value1", first);
        Assert.Equal("value2", second);
    }

    [Fact]
    public void GivenSetupSequenceWithAsyncReturns_WhenCallingAsyncMethodMultipleTimes_ThenReturnsSequentialValues()
    {
        // Arrange
        _sut.SetupSequence(x => x.GetDataAsync())
            .Returns(Task.FromResult("first"))
            .Returns(Task.FromResult("second"));

        // Act
        var first = _sut.Object.GetDataAsync().Result;
        var second = _sut.Object.GetDataAsync().Result;

        // Assert
        Assert.Equal("first", first);
        Assert.Equal("second", second);
    }

    // SetupSequence Tests with Throws
    [Fact]
    public void GivenSetupSequenceWithThrows_WhenCallingMethodMultipleTimes_ThenThrowsAndReturnsSequentially()
    {
        // Arrange
        _sut.SetupSequence(x => x.Process("test"))
            .Throws<InvalidOperationException>()
            .Returns("success");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.Process("test"));
        var actual = _sut.Object.Process("test");
        Assert.Equal("success", actual);
    }

    [Fact]
    public void GivenSetupSequenceWithThrowsInstance_WhenCallingMethodMultipleTimes_ThenThrowsAndReturnsSequentially()
    {
        // Arrange
        var expectedException = new ArgumentException("First call error");
        _sut.SetupSequence(x => x.Process("test"))
            .Throws(expectedException)
            .Returns("success");

        // Act & Assert
        var actualException = Assert.Throws<ArgumentException>(() => _sut.Object.Process("test"));
        Assert.Equal("First call error", actualException.Message);
        
        var actual = _sut.Object.Process("test");
        Assert.Equal("success", actual);
    }

    [Fact]
    public void GivenSetupSequenceForVoidMethod_WhenCallingMethodMultipleTimes_ThenThrowsSequentially()
    {
        // Arrange
        _sut.SetupSequence(x => x.ProcessWithDefaults("test", 42, true))
            .Throws<InvalidOperationException>()
            .Callback(() => { }); // No exception on second call

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _sut.Object.ProcessWithDefaults("test"));
        
        var exception = Record.Exception(() => _sut.Object.ProcessWithDefaults("test"));
        Assert.Null(exception);
    }

    // Mixed Sequence Tests
    [Fact]
    public void GivenSetupSequenceWithMixedReturnsAndThrows_WhenCallingMethod_ThenFollowsSequence()
    {
        // Arrange
        _sut.SetupSequence(x => x.Process("test"))
            .Returns("first")
            .Throws<InvalidOperationException>()
            .Returns("third")
            .Throws<ArgumentException>();

        // Act & Assert
        var first = _sut.Object.Process("test");
        Assert.Equal("first", first);

        Assert.Throws<InvalidOperationException>(() => _sut.Object.Process("test"));

        var third = _sut.Object.Process("test");
        Assert.Equal("third", third);

        Assert.Throws<ArgumentException>(() => _sut.Object.Process("test"));
    }

    // Generic Method Setup Tests
    [Fact]
    public void GivenSetupForGenericMethod_WhenCallingWithMatchingType_ThenReturnsConfiguredValue()
    {
        // Arrange
        var input = "test input";
        var expectedOutput = "processed output";
        _sut.Setup(x => x.Process(input)).Returns(expectedOutput);

        // Act
        var actual = _sut.Object.Process(input);

        // Assert
        Assert.Equal(expectedOutput, actual);
    }

    [Fact]
    public void GivenSetupForGenericMethodWithConstraints_WhenCallingWithMatchingType_ThenReturnsConfiguredValue()
    {
        // Arrange
        var input = new List<int> { 1, 2, 3 };
        var expectedOutput = "transformed";
        _sut.Setup(x => x.Transform<List<int>, string>(input)).Returns(expectedOutput);

        // Act
        var actual = _sut.Object.Transform<List<int>, string>(input);

        // Assert
        Assert.Equal(expectedOutput, actual);
    }
}