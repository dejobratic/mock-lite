using MockLite.Tests.Unit.Sample.Domain;

namespace MockLite.Tests.Unit.Sample.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer> CreateAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<List<Customer>> GetByTypeAsync(CustomerType type);
    bool IsEmailUnique(string email);
    int ActiveCustomersCount { get; }
}