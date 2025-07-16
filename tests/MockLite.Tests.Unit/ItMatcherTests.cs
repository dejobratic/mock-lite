// ReSharper disable MemberCanBePrivate.Global
namespace MockLite.Tests.Unit;

public class ItMatcherTests
{
    public interface ICalculator
    {
        int Add(int a, int b);
        
        string FormatNumber(int number);
        
        bool IsEven(int number);
        
        decimal Calculate(decimal amount, string operation);
    }

    private readonly Mock<ICalculator> _sut = new();

    [Fact]
    public void GivenSetupWithItIsAny_WhenCalledWithAnyValue_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(100);

        // Act
        var actual = _sut.Object.Add(5, 10);

        // Assert
        Assert.Equal(100, actual);
    }

    [Fact]
    public void GivenSetupWithItIsAny_WhenCalledWithDifferentValues_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(42);

        // Act
        var result1 = _sut.Object.Add(1, 2);
        var result2 = _sut.Object.Add(999, -500);

        // Assert
        Assert.Equal(42, result1);
        Assert.Equal(42, result2);
    }

    [Fact]
    public void GivenSetupWithSpecificValueAndItIsAny_WhenCalledWithMatchingSpecificValue_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Add(5, It.IsAny<int>()))
            .Returns(200);

        // Act
        var actual = _sut.Object.Add(5, 999);

        // Assert
        Assert.Equal(200, actual);
    }

    [Fact]
    public void GivenSetupWithSpecificValueAndItIsAny_WhenCalledWithDifferentSpecificValue_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.Add(5, It.IsAny<int>()))
            .Returns(200);

        // Act
        var actual = _sut.Object.Add(10, 999);

        // Assert
        Assert.Equal(0, actual); // Default value
    }

    [Fact]
    public void GivenSetupWithItIs_WhenCalledWithMatchingPredicate_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Add(It.Is<int>(n => n > 0), It.Is<int>(n => n < 100)))
            .Returns(500);

        // Act
        var actual = _sut.Object.Add(5, 10);

        // Assert
        Assert.Equal(500, actual);
    }

    [Fact]
    public void GivenSetupWithItIs_WhenCalledWithNonMatchingPredicate_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.Add(It.Is<int>(n => n > 0), It.Is<int>(n => n < 100)))
            .Returns(500);

        // Act
        var actual = _sut.Object.Add(-5, 10); // The first parameter doesn't match predicate

        // Assert
        Assert.Equal(0, actual); // Default value
    }

    [Fact]
    public void GivenSetupWithItIsForString_WhenCalledWithMatchingString_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.FormatNumber(It.Is<int>(n => n > 0)))
            .Returns("Positive");

        // Act
        var actual = _sut.Object.FormatNumber(42);

        // Assert
        Assert.Equal("Positive", actual);
    }

    [Fact]
    public void GivenSetupWithItIsForString_WhenCalledWithNonMatchingString_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.FormatNumber(It.Is<int>(n => n > 0)))
            .Returns("Positive");

        // Act
        var actual = _sut.Object.FormatNumber(-5);

        // Assert
        Assert.Null(actual); // Default value for string
    }

    [Fact]
    public void GivenSetupWithComplexItIs_WhenCalledWithMatchingCondition_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.IsEven(It.Is<int>(n => n % 2 == 0 && n > 0)))
            .Returns(true);

        // Act
        var actual = _sut.Object.IsEven(4);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithComplexItIs_WhenCalledWithNonMatchingCondition_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.IsEven(It.Is<int>(n => n % 2 == 0 && n > 0)))
            .Returns(true);

        // Act
        var actual = _sut.Object.IsEven(3); // Odd number

        // Assert
        Assert.False(actual); // Default value for bool
    }

    [Fact]
    public void GivenSetupWithMixedMatchers_WhenCalledWithMatchingValues_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Calculate(It.Is<decimal>(d => d > 0), It.IsAny<string>()))
            .Returns(999.99m);

        // Act
        var actual = _sut.Object.Calculate(100.50m, "multiply");

        // Assert
        Assert.Equal(999.99m, actual);
    }

    [Fact]
    public void GivenSetupWithMixedMatchers_WhenCalledWithNonMatchingValues_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.Calculate(It.Is<decimal>(d => d > 0), It.IsAny<string>()))
            .Returns(999.99m);

        // Act
        var actual = _sut.Object.Calculate(-100.50m, "multiply"); // Negative amount doesn't match

        // Assert
        Assert.Equal(0m, actual); // Default value for decimal
    }

    [Fact]
    public void GivenSetupWithItIsAnyString_WhenCalledWithDifferentStrings_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<string>()))
            .Returns(123.45m);

        // Act
        var result1 = _sut.Object.Calculate(50m, "add");
        var result2 = _sut.Object.Calculate(75m, "subtract");

        // Assert
        Assert.Equal(123.45m, result1);
        Assert.Equal(123.45m, result2);
    }

    [Fact]
    public void GivenSetupWithItIsString_WhenCalledWithMatchingString_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Calculate(It.IsAny<decimal>(), It.Is<string>(s => s.StartsWith("multi"))))
            .Returns(999m);

        // Act
        var actual = _sut.Object.Calculate(100m, "multiply");

        // Assert
        Assert.Equal(999m, actual);
    }

    [Fact]
    public void GivenSetupWithItIsString_WhenCalledWithNonMatchingString_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.Calculate(It.IsAny<decimal>(), It.Is<string>(s => s.StartsWith("multi"))))
            .Returns(999m);

        // Act
        var actual = _sut.Object.Calculate(100m, "divide");

        // Assert
        Assert.Equal(0m, actual); // Default value
    }

    [Fact]
    public void GivenMultipleSetupsWithDifferentMatchers_WhenCalledWithMatchingValues_ThenCorrectSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Add(It.Is<int>(n => n > 10), It.IsAny<int>()))
            .Returns(100);
        
        _sut.Setup(x => x.Add(It.Is<int>(n => n <= 10), It.IsAny<int>()))
            .Returns(200);

        // Act
        var result1 = _sut.Object.Add(15, 5); // First setup
        var result2 = _sut.Object.Add(5, 15);  // Second setup

        // Assert
        Assert.Equal(100, result1);
        Assert.Equal(200, result2);
    }
}