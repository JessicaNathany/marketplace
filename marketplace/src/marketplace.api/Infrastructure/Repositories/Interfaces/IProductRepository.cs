using marketplace.api.Domain.Entities;
namespace marketplace.api.Infrastructure.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(bool asNoTracking = false);
    Task<Product?> GetByKeyAsync(string name, string brand, string category);
    Task<List<Product>> GetByFiltersAsync(string? name, string? brand, string? category, int page, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(Product product);
}    
