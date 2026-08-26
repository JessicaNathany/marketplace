using marketplace.api.Domain.Entities;
using marketplace.api.Infrastructure.Data;
using marketplace.api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace marketplace.api.Infrastructure.Repositories;
public class SellerProductRepository(MarketplaceDbContext context) : ISellerProductRepository
{
    public async Task AddAsync(SellerProduct sellerProduct)
    {
        await context.Set<SellerProduct>().AddAsync(sellerProduct);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string sellerName, string externalId)
    {
        var query = context.SellerProducts
            .Where(sp => sp.SellerName == sellerName && sp.SellerProductId == externalId);
        return await query.AnyAsync();
    }

    public async Task<List<SellerProduct>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.SellerProducts
            .AsNoTracking()
            .OrderBy(sp => sp.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SellerProduct>> GetBySellerNameAsync(string sellerName, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.SellerProducts
            .AsNoTracking()
            .Where(sp => sp.SellerName == sellerName)
            .OrderBy(sp => sp.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
