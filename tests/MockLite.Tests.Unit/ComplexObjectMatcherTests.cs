// ReSharper disable MemberCanBePrivate.Global
namespace MockLite.Tests.Unit;

public class ComplexObjectMatcherTests
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsProcessed { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public decimal CreditLimit { get; set; }
    }

    public interface IOrderService
    {
        bool ValidateOrder(Order order);
        
        decimal CalculateDiscount(Order order, Customer customer);
        
        string ProcessOrder(Order order);
        
        bool NotifyCustomer(Customer customer, string message);
    }

    private readonly Mock<IOrderService> _sut = new();

    [Fact]
    public void GivenSetupWithItIsAnyOrder_WhenCalledWithAnyOrder_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.IsAny<Order>()))
            .Returns(true);

        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 100.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsAnyCustomer_WhenCalledWithDifferentCustomers_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.NotifyCustomer(It.IsAny<Customer>(), It.IsAny<string>()))
            .Returns(true);

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 1000.00m
        };

        // Act
        var actual = _sut.Object.NotifyCustomer(customer, "Welcome");

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsForOrderId_WhenCalledWithMatchingOrderId_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => o.Id > 0)))
            .Returns(true);

        var order = new Order
        {
            Id = 123,
            CustomerName = "John Doe",
            TotalAmount = 100.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsForOrderId_WhenCalledWithNonMatchingOrderId_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => o.Id > 0)))
            .Returns(true);

        var order = new Order
        {
            Id = -1,
            CustomerName = "John Doe",
            TotalAmount = 100.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.False(actual); // Default value
    }

    [Fact]
    public void GivenSetupWithItIsForTotalAmount_WhenCalledWithMatchingAmount_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => o.TotalAmount >= 50.00m)))
            .Returns(true);

        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 75.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsForTotalAmount_WhenCalledWithNonMatchingAmount_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => o.TotalAmount >= 50.00m)))
            .Returns(true);

        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 25.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.False(actual); // Default value
    }

    [Fact]
    public void GivenSetupWithItIsForCustomerName_WhenCalledWithMatchingName_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => o.CustomerName!.StartsWith("John"))))
            .Returns(true);

        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 100.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsForCustomerName_WhenCalledWithNonMatchingName_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => o.CustomerName!.StartsWith("John"))))
            .Returns(true);

        var order = new Order
        {
            Id = 1,
            CustomerName = "Jane Smith",
            TotalAmount = 100.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.False(actual); // Default value
    }

    [Fact]
    public void GivenSetupWithItIsForIsProcessed_WhenCalledWithMatchingProcessedStatus_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => !o.IsProcessed)))
            .Returns(true);

        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 100.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsForIsProcessed_WhenCalledWithNonMatchingProcessedStatus_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => !o.IsProcessed)))
            .Returns(true);

        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 100.00m,
            OrderDate = DateTime.Now,
            IsProcessed = true
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.False(actual); // Default value
    }

    [Fact]
    public void GivenSetupWithComplexItIsForMultipleProperties_WhenCalledWithMatchingProperties_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => 
            o.Id > 0 && 
            o.TotalAmount >= 50.00m && 
            o.CustomerName!.Contains("John") && 
            !o.IsProcessed)))
            .Returns(true);

        var order = new Order
        {
            Id = 123,
            CustomerName = "John Doe",
            TotalAmount = 75.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithComplexItIsForMultipleProperties_WhenCalledWithNonMatchingProperties_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => 
            o.Id > 0 && 
            o.TotalAmount >= 50.00m && 
            o.CustomerName!.Contains("John") && 
            !o.IsProcessed)))
            .Returns(true);

        var order = new Order
        {
            Id = 123,
            CustomerName = "Jane Smith", // This doesn't match
            TotalAmount = 75.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        // Act
        var actual = _sut.Object.ValidateOrder(order);

        // Assert
        Assert.False(actual); // Default value
    }

    [Fact]
    public void GivenSetupWithItIsForActiveCustomer_WhenCalledWithActiveCustomer_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.NotifyCustomer(It.Is<Customer>(c => c.IsActive), It.IsAny<string>()))
            .Returns(true);

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 1000.00m
        };

        // Act
        var actual = _sut.Object.NotifyCustomer(customer, "Welcome");

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsForActiveCustomer_WhenCalledWithInactiveCustomer_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.NotifyCustomer(It.Is<Customer>(c => c.IsActive), It.IsAny<string>()))
            .Returns(true);

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = false,
            CreditLimit = 1000.00m
        };

        // Act
        var actual = _sut.Object.NotifyCustomer(customer, "Welcome");

        // Assert
        Assert.False(actual); // Default value
    }

    [Fact]
    public void GivenSetupWithItIsForCustomerCreditLimit_WhenCalledWithMatchingCreditLimit_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.NotifyCustomer(It.Is<Customer>(c => c.CreditLimit >= 500.00m), It.IsAny<string>()))
            .Returns(true);

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 750.00m
        };

        // Act
        var actual = _sut.Object.NotifyCustomer(customer, "Premium customer");

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsForCustomerCreditLimit_WhenCalledWithNonMatchingCreditLimit_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.NotifyCustomer(It.Is<Customer>(c => c.CreditLimit >= 500.00m), It.IsAny<string>()))
            .Returns(true);

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 250.00m
        };

        // Act
        var actual = _sut.Object.NotifyCustomer(customer, "Premium customer");

        // Assert
        Assert.False(actual); // Default value
    }

    [Fact]
    public void GivenSetupWithItIsForCustomerEmail_WhenCalledWithMatchingEmailDomain_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.NotifyCustomer(It.Is<Customer>(c => c.Email.EndsWith("@example.com")), It.IsAny<string>()))
            .Returns(true);

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 1000.00m
        };

        // Act
        var actual = _sut.Object.NotifyCustomer(customer, "Domain verified");

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GivenSetupWithItIsForCustomerEmail_WhenCalledWithNonMatchingEmailDomain_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.NotifyCustomer(It.Is<Customer>(c => c.Email.EndsWith("@example.com")), It.IsAny<string>()))
            .Returns(true);

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@gmail.com",
            IsActive = true,
            CreditLimit = 1000.00m
        };

        // Act
        var actual = _sut.Object.NotifyCustomer(customer, "Domain verified");

        // Assert
        Assert.False(actual); // Default value
    }

    [Fact]
    public void GivenSetupWithMixedMatchersForDiscount_WhenCalledWithMatchingValues_ThenSetupMatches()
    {
        // Arrange
        _sut.Setup(x => x.CalculateDiscount(
            It.Is<Order>(o => o.TotalAmount > 100.00m),
            It.Is<Customer>(c => c.IsActive)))
            .Returns(10.00m);

        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 150.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 1000.00m
        };

        // Act
        var actual = _sut.Object.CalculateDiscount(order, customer);

        // Assert
        Assert.Equal(10.00m, actual);
    }

    [Fact]
    public void GivenSetupWithMixedMatchersForDiscount_WhenCalledWithNonMatchingValues_ThenSetupDoesNotMatch()
    {
        // Arrange
        _sut.Setup(x => x.CalculateDiscount(
            It.Is<Order>(o => o.TotalAmount > 100.00m),
            It.Is<Customer>(c => c.IsActive)))
            .Returns(10.00m);

        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 75.00m, // This doesn't match
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 1000.00m
        };

        // Act
        var actual = _sut.Object.CalculateDiscount(order, customer);

        // Assert
        Assert.Equal(0m, actual); // Default value
    }

    [Fact]
    public void GivenOrderValidated_WhenVerifyingWithItIsForOrderId_ThenVerificationSucceeds()
    {
        // Arrange
        var order = new Order
        {
            Id = 123,
            CustomerName = "John Doe",
            TotalAmount = 100.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        _sut.Setup(x => x.ValidateOrder(It.Is<Order>(o => o.Id == 123)))
            .Returns(true);

        // Act
        _sut.Object.ValidateOrder(order);

        // Assert
        _sut.Verify(x => x.ValidateOrder(It.Is<Order>(o => o.Id == 123)), Times.Once);
    }

    [Fact]
    public void GivenCustomerNotified_WhenVerifyingWithItIsForActiveCustomer_ThenVerificationSucceeds()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 1000.00m
        };

        _sut.Setup(x => x.NotifyCustomer(It.Is<Customer>(c => c.IsActive), It.IsAny<string>()))
            .Returns(true);

        // Act
        _sut.Object.NotifyCustomer(customer, "Welcome");

        // Assert
        _sut.Verify(x => x.NotifyCustomer(It.Is<Customer>(c => c.IsActive), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GivenDiscountCalculated_WhenVerifyingWithComplexObjectMatchers_ThenVerificationSucceeds()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            TotalAmount = 150.00m,
            OrderDate = DateTime.Now,
            IsProcessed = false
        };

        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true,
            CreditLimit = 1000.00m
        };

        _sut.Setup(x => x.CalculateDiscount(
            It.Is<Order>(o => o.TotalAmount > 100.00m),
            It.Is<Customer>(c => c.IsActive)))
            .Returns(10.00m);

        // Act
        _sut.Object.CalculateDiscount(order, customer);

        // Assert
        _sut.Verify(x => x.CalculateDiscount(
            It.Is<Order>(o => o.TotalAmount > 100.00m && o.CustomerName!.Contains("John")),
            It.Is<Customer>(c => c.IsActive && c.CreditLimit >= 500.00m)), Times.Once);
    }
}