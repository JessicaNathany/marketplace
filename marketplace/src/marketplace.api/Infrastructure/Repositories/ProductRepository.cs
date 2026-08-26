using marketplace.api.Domain.Entities;
using marketplace.api.Infrastructure.Data;
using marketplace.api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace marketplace.api.Infrastructure.Repositories;

public class ProductRepository(MarketplaceDbContext context) : IProductRepository
{
    public async Task AddAsync(Product product)
    {
        await context.Set<Product>().AddAsync(product);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync(bool asNoTracking = false)
    {
        var query = context.Set<Product>();

        if (asNoTracking)
            return await query.AsNoTracking().ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<Product?> GetByKeyAsync(string name, string brand, string category)
    {
        var query = context.Products
            .Where(p => p.Name == name 
            && p.Brand == brand
            && p.Category == category);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetByFiltersAsync(
        string? name,
        string? brand,
        string? category,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(p => p.Brand.Contains(brand));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category.Contains(category));
        }

        return await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
