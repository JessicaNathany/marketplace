using marketplace.api.Domain.Entities;
using marketplace.api.Infrastructure.Repositories.Interfaces;
using marketplace.api.Service.Interfaces;
namespace marketplace.api.Service;

public class SellerProductService(ISellerProductRepository sellerProductRepository) : ISellerProductService
{
    public Task<List<SellerProduct>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return sellerProductRepository.GetAllAsync(page, pageSize, cancellationToken);
    }

    public Task<List<SellerProduct>> GetBySellerNameAsync(string sellerName, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sellerName))
        {
            return Task.FromResult(new List<SellerProduct>());
        }

        var normalizedSellerName = sellerName.Trim();
        return sellerProductRepository.GetBySellerNameAsync(normalizedSellerName, page, pageSize, cancellationToken);
    }
}
