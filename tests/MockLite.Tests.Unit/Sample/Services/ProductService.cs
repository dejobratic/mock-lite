using MockLite.Tests.Unit.Sample.Domain;
using MockLite.Tests.Unit.Sample.Interfaces;

namespace MockLite.Tests.Unit.Sample.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly INotificationService _notificationService;

    public ProductService(IProductRepository productRepository, INotificationService notificationService)
    {
        _productRepository = productRepository;
        _notificationService = notificationService;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required", nameof(product));

        if (product.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero", nameof(product));

        product.IsActive = true;
        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty", nameof(category));

        return await _productRepository.GetByCategoryAsync(category);
    }

    public async Task UpdateStockAsync(int productId, int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(newQuantity));

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {productId} not found");

        var oldQuantity = product.StockQuantity;
        var updated = await _productRepository.UpdateStockAsync(productId, newQuantity);
        
        if (!updated)
            throw new InvalidOperationException("Failed to update stock");

        // Send low stock alert if needed
        if (newQuantity <= 5 && oldQuantity > 5)
        {
            product.StockQuantity = newQuantity;
            await _notificationService.SendLowStockAlertAsync(product);
        }
    }

    public async Task<bool> IsProductAvailableAsync(int productId, int requestedQuantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        return product?.IsActive == true && product.StockQuantity >= requestedQuantity;
    }

    public async Task DeactivateProductAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {productId} not found");

        if (!product.IsActive)
            throw new InvalidOperationException("Product is already inactive");

        product.IsActive = false;
        // Note: In real implementation, this would call UpdateAsync
        // For testing purposes, we'll assume the repository handles this
    }
}