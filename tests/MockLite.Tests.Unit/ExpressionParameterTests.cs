using System.Linq.Expressions;
// ReSharper disable MemberCanBePrivate.Global

namespace MockLite.Tests.Unit;

public class ExpressionParameterTests
{
    public interface IProductService
    {
        // Repository-style methods
        IEnumerable<Product> Find(Expression<Func<Product, bool>> predicate);
        
        Product? FindSingle(Expression<Func<Product, bool>> predicate);
        
        bool Any(Expression<Func<Product, bool>> predicate);
        
        int Count(Expression<Func<Product, bool>> predicate);
        
        void Update(Product entity, Expression<Func<Product, object>> propertyExpression);
        
        void Delete(Expression<Func<Product, bool>> predicate);
        
        // Query-style methods with additional parameters
        IEnumerable<Product> Where(IQueryable<Product> source, Expression<Func<Product, bool>> predicate);
        
        IEnumerable<string> SelectNames(IQueryable<Product> source, Expression<Func<Product, string>> selector);
        
        IOrderedQueryable<Product> OrderByPrice(IQueryable<Product> source, Expression<Func<Product, decimal>> keySelector);
        
        Product? FirstOrDefault(IQueryable<Product> source, Expression<Func<Product, bool>> predicate);
    }

    public class Product
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public decimal Price { get; init; }
        public string? Category { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedDate { get; init; }
    }

    private readonly Mock<IProductService> _sut = new();

    [Fact]
    public void GivenSetupWithBasicExpressionPredicate_WhenCalledWithMatchingExpression_ThenSetupMatches()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new() { Id = 1, Name = "Product1", Price = 100m, Category = "Electronics", IsActive = true, CreatedDate = DateTime.Now },
            new() { Id = 2, Name = "Product2", Price = 200m, Category = "Electronics", IsActive = true, CreatedDate = DateTime.Now }
        };

        _sut.Setup(x => x.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(expectedProducts);

        // Act
        var actual = _sut.Object.Find(p => p.Category == "Electronics");

        // Assert
        Assert.Equal(expectedProducts, actual);
    }

    [Fact]
    public void GivenSetupWithSpecificExpressionMatch_WhenCalledWithExactExpression_ThenSetupMatches()
    {
        // Arrange
        var expectedProduct = new Product 
        { 
            Id = 1, 
            Name = "Test Product", 
            Price = 99.99m, 
            Category = "Test", 
            IsActive = true, 
            CreatedDate = DateTime.Now 
        };

        _sut.Setup(x => x.FindSingle(It.Is<Expression<Func<Product, bool>>>(
            expr => expr.ToString().Contains("Id") && expr.ToString().Contains("1"))))
            .Returns(expectedProduct);

        // Act
        var actual = _sut.Object.FindSingle(p => p.Id == 1);

        // Assert
        Assert.Equal(expectedProduct, actual);
    }

    [Fact]
    public void GivenSetupWithExpressionForAnyMethod_WhenCalledWithPredicate_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Any(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(true);

        // Act
        var actual = _sut.Object.Any(p => p.Price > 50m);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithExpressionForCountMethod_WhenCalledWithPredicate_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Count(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(5);

        // Act
        var actual = _sut.Object.Count(p => p.IsActive);

        // Assert
        Assert.Equal(5, actual);
    }

    [Fact]
    public void GivenSetupWithPropertyExpression_WhenCalledWithPropertySelector_ThenSetupMatches()
    {
        // Arrange
        var product = new Product 
        { 
            Id = 1, 
            Name = "Test Product", 
            Price = 99.99m, 
            Category = "Test", 
            IsActive = true, 
            CreatedDate = DateTime.Now 
        };

        // No setup needed for void method

        // Act
        _sut.Object.Update(product, p => p.Name!);

        // Assert
        _sut.Verify(x => x.Update(
            It.IsAny<Product>(), 
            It.IsAny<Expression<Func<Product, object>>>()), Times.Once);
    }

    [Fact]
    public void GivenSetupWithDeleteExpression_WhenCalledWithPredicate_ThenSetupMatches()
    {
        // Arrange
        // No setup needed for void method

        // Act
        _sut.Object.Delete(p => p.Id == 999);

        // Assert
        _sut.Verify(x => x.Delete(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once);
    }


    [Fact]
    public void GivenSetupWithSelectExpression_WhenCalledWithSelector_ThenSetupMatches()
    {
        // Arrange
        var source = new List<Product>().AsQueryable();
        var expectedResults = new List<string> { "Product1", "Product2" };

        _sut.Setup(x => x.SelectNames(
            It.IsAny<IQueryable<Product>>(), 
            It.IsAny<Expression<Func<Product, string>>>()))
            .Returns(expectedResults);

        // Act
        var actual = _sut.Object.SelectNames(source, p => p.Name ?? string.Empty);

        // Assert
        Assert.Equal(expectedResults, actual);
    }

    [Fact]
    public void GivenSetupWithOrderByExpression_WhenCalledWithKeySelector_ThenSetupMatches()
    {
        // Arrange
        var source = new List<Product>().AsQueryable();
        var expectedResult = source.OrderBy(p => p.Price);

        _sut.Setup(x => x.OrderByPrice(
            It.IsAny<IQueryable<Product>>(), 
            It.IsAny<Expression<Func<Product, decimal>>>()))
            .Returns(expectedResult);

        // Act
        var actual = _sut.Object.OrderByPrice(source, p => p.Price);

        // Assert
        Assert.Equal(expectedResult, actual);
    }


    [Fact]
    public void GivenMultipleSetups_WhenCalledWithExpressions_ThenCorrectSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Count(It.Is<Expression<Func<Product, bool>>>(
            expr => expr.ToString().Contains("IsActive"))))
            .Returns(10);

        _sut.Setup(x => x.Count(It.Is<Expression<Func<Product, bool>>>(
            expr => expr.ToString().Contains("Price"))))
            .Returns(5);

        // Act
        var activeCount = _sut.Object.Count(p => p.IsActive);
        var priceCount = _sut.Object.Count(p => p.Price > 100m);

        // Assert
        Assert.Equal(10, activeCount);
        Assert.Equal(5, priceCount);
    }

    [Fact]
    public void GivenSetupWithExpressionVerification_WhenMethodCalled_ThenVerificationSucceeds()
    {
        // Arrange
        var product = new Product 
        { 
            Id = 1, 
            Name = "Test Product", 
            Price = 99.99m, 
            Category = "Test", 
            IsActive = true, 
            CreatedDate = DateTime.Now 
        };

        // Act
        _sut.Object.Update(product, p => p.Name!);

        // Assert
        _sut.Verify(x => x.Update(
            It.Is<Product>(p => p.Id == 1), 
            It.IsAny<Expression<Func<Product, object>>>()), Times.Once);
    }

    [Fact]
    public void GivenSetupWithComplexExpressionPredicate_WhenCalledWithMatchingComplexExpression_ThenSetupMatches()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new() { Id = 1, Name = "Premium Product", Price = 500m, Category = "Electronics", IsActive = true, CreatedDate = DateTime.Now.AddDays(-10) }
        };

        _sut.Setup(x => x.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(expectedProducts);

        // Act
        var actual = _sut.Object.Find(p => 
            p.Price > 100m && 
            p.IsActive && 
            p.Category == "Electronics" && 
            p.CreatedDate > DateTime.Now.AddDays(-30));

        // Assert
        Assert.Equal(expectedProducts, actual);
    }

    [Fact]
    public void GivenSetupWithNullExpressionHandling_WhenCalledWithNullPredicate_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(new List<Product>());

        // Act & Assert
        Assert.NotNull(_sut.Object.Find(p => p.Name != null));
    }

    [Fact]
    public void GivenSetupWithExpressionStringContains_WhenCalledWithStringExpression_ThenSetupMatches()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new() { Id = 1, Name = "Test Product", Price = 100m, Category = "Test Category", IsActive = true, CreatedDate = DateTime.Now }
        };

        _sut.Setup(x => x.Find(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(expectedProducts);

        // Act
        var actual = _sut.Object.Find(p => p.Name!.Contains("Test") && p.Category!.StartsWith("Test"));

        // Assert
        Assert.Equal(expectedProducts, actual);
    }

    [Fact]
    public void GivenSetupWithQueryStyleMethods_WhenCalledWithSource_ThenSetupMatches()
    {
        // Arrange
        var source = new List<Product>().AsQueryable();
        var expectedProducts = new List<Product>
        {
            new() { Id = 1, Name = "Active Product", Price = 100m, Category = "Test", IsActive = true, CreatedDate = DateTime.Now }
        };

        _sut.Setup(x => x.Where(
            It.IsAny<IQueryable<Product>>(), 
            It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(expectedProducts);

        // Act
        var actual = _sut.Object.Where(source, p => p.IsActive);

        // Assert
        Assert.Equal(expectedProducts, actual);
    }

    [Fact]
    public void GivenSetupWithFirstOrDefaultQueryMethod_WhenCalledWithPredicate_ThenSetupMatches()
    {
        // Arrange
        var source = new List<Product>().AsQueryable();
        var expectedProduct = new Product 
        { 
            Id = 1, 
            Name = "First Product", 
            Price = 50m, 
            Category = "Test", 
            IsActive = true, 
            CreatedDate = DateTime.Now 
        };

        _sut.Setup(x => x.FirstOrDefault(
            It.IsAny<IQueryable<Product>>(), 
            It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(expectedProduct);

        // Act
        var actual = _sut.Object.FirstOrDefault(source, p => p.Id == 1);

        // Assert
        Assert.Equal(expectedProduct, actual);
    }
}