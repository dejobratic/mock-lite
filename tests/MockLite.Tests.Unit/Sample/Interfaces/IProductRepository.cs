using MockLite.Tests.Unit.Sample.Domain;

namespace MockLite.Tests.Unit.Sample.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> GetByCategoryAsync(string category);
    Task<bool> UpdateStockAsync(int productId, int newQuantity);
    Task<Product> CreateAsync(Product product);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    int GetInStockCount();
    Task<List<Product>> GetAllAsync();
    decimal GetAveragePrice();
}