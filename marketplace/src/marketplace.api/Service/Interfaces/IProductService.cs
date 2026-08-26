using marketplace.api.Domain.Dto;
using marketplace.api.Domain.Entities;

namespace marketplace.api.Service.Interfaces;
public interface IProductService
{
    Task<CatalogImportResultDto> ProcessCatalogAddProduct(CancellationToken cancellationToken = default);
    Task<List<Product>> GetProductsAsync(string? name, string? brand, string? category, int page, int pageSize, CancellationToken cancellationToken = default);
}
