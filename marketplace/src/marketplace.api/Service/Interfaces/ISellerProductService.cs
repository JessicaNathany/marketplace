using marketplace.api.Domain.Entities;

namespace marketplace.api.Service.Interfaces;
public interface ISellerProductService
{
    Task<List<SellerProduct>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<List<SellerProduct>> GetBySellerNameAsync(string sellerName, int page, int pageSize, CancellationToken cancellationToken = default);
}
