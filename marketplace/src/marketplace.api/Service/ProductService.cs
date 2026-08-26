using marketplace.api.Domain.Dto;
using marketplace.api.Domain.Entities;
using marketplace.api.Infrastructure.Repositories.Interfaces;
using marketplace.api.Service.Interfaces;

namespace marketplace.api.Service;

public class ProductService(
    ICatalogService catalogService,
    IProductRepository productRepository,
    ISellerProductRepository sellerProductRepository) : IProductService
{
    public async Task<CatalogImportResultDto> ProcessCatalogAddProduct(CancellationToken cancellationToken = default)
    {
        var catalogImport = await catalogService.ImportCatalogAsync(1, int.MaxValue, cancellationToken);

        var createdProducts = 0;
        var linkedSellerProducts = 0;
        var ignoredDuplicates = 0;

        foreach (var item in catalogImport.Items)
        {
            var existingProduct = await productRepository.GetByKeyAsync(item.Name, item.Brand, item.Category);
            
            if (existingProduct is null)
            {
                existingProduct = new Product(item.Name, item.Brand, item.Category);
                await productRepository.AddAsync(existingProduct);
                createdProducts++;
            }

            var alreadyLinked = await sellerProductRepository.ExistsAsync(item.SellerName, item.Id);
            
            if (alreadyLinked)
            {
                ignoredDuplicates++;
                continue;
            }

            var sellerProduct = new SellerProduct(item.SellerName, existingProduct.Id, item.Id);
            await sellerProductRepository.AddAsync(sellerProduct);
            linkedSellerProducts++;
        }

        return new CatalogImportResultDto
        {
            Page = 1,
            PageSize = catalogImport.TotalItems,
            TotalItems = catalogImport.TotalItems,
            TotalPages = catalogImport.TotalItems > 0 ? 1 : 0,
            Processed = catalogImport.TotalItems,
            InvalidItems = catalogImport.InvalidItems,
            ItemsWithMissingBrand = catalogImport.ItemsWithMissingBrand,
            CreatedProducts = createdProducts,
            LinkedSellerProducts = linkedSellerProducts,
            IgnoredDuplicates = ignoredDuplicates,
            Items = []
        };
    }

    public Task<List<Product>> GetProductsAsync(
        string? name,
        string? brand,
        string? category,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return productRepository.GetByFiltersAsync(
            NormalizeFilter(name),
            NormalizeFilter(brand),
            NormalizeFilter(category),
            page,
            pageSize,
            cancellationToken);
    }

    private static string? NormalizeFilter(string? value)
    {
        var trimmed = value?.Trim();
        return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
    }
}
