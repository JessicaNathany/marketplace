using marketplace.api.Domain.Entities;

namespace marketplace.api.Infrastructure.Repositories.Interfaces;
public interface ISellerProductRepository
{
    Task<bool> ExistsAsync(string sellerName, string externalId);
    Task AddAsync(SellerProduct sellerProduct);

    Task<List<SellerProduct>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<List<SellerProduct>> GetBySellerNameAsync(string sellerName, int page, int pageSize, CancellationToken cancellationToken = default);
}
