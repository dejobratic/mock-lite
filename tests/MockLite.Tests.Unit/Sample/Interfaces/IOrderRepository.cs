using MockLite.Tests.Unit.Sample.Domain;

namespace MockLite.Tests.Unit.Sample.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByCustomerIdAsync(int customerId);
    Task<Order> CreateAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);
    Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<decimal> GetTotalRevenueAsync();
    int GetTotalOrderCount();
}