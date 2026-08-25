using marketplace.api.Domain.Dto;
using marketplace.api.Service.Interfaces;
using System.Text.Json;

namespace marketplace.api.Service;
public class CatalogService(IWebHostEnvironment environment, ILogger<CatalogService> logger) : ICatalogService
{
    public async Task<CatalogImportResultDto> ImportCatalogAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(environment.ContentRootPath, "Domain", "import-seller-product.json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Catalog input file was not found.", filePath);
        }

        var jsonContent = await File.ReadAllTextAsync(filePath, cancellationToken);

        var importedItems = JsonSerializer.Deserialize<List<ImportedSellerProductDto>>(
            jsonContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        ) ?? new List<ImportedSellerProductDto>();

        var normalizedItems = importedItems
            .Select(item => new ImportedSellerProductDto
            {
                Id = Normalize(item.Id),
                SellerName = Normalize(item.SellerName),
                Name = Normalize(item.Name),
                Brand = Normalize(item.Brand),
                Category = Normalize(item.Category)
            })
            .ToList();

        var invalidItems = normalizedItems
            .Where(item =>
                string.IsNullOrWhiteSpace(item.Id) ||
                string.IsNullOrWhiteSpace(item.SellerName) ||
                string.IsNullOrWhiteSpace(item.Name))
            .ToList();

        foreach (var invalidItem in invalidItems)
        {
            logger.LogWarning(
                "Catalog item ignored due to missing required data. ExternalId: {ExternalId}, SellerName: {SellerName}, Name: {Name}",
                invalidItem.Id,
                invalidItem.SellerName,
                invalidItem.Name);
        }

        var validItems = normalizedItems
            .Where(item =>
                !string.IsNullOrWhiteSpace(item.Id) &&
                !string.IsNullOrWhiteSpace(item.SellerName) &&
                !string.IsNullOrWhiteSpace(item.Name))
            .ToList();

        var itemsWithMissingBrand = validItems
            .Where(item => string.IsNullOrWhiteSpace(item.Brand))
            .ToList();

        foreach (var incompleteItem in itemsWithMissingBrand)
        {
            logger.LogInformation(
                "Catalog item imported with missing brand. ExternalId: {ExternalId}, SellerName: {SellerName}, Name: {Name}",
                incompleteItem.Id,
                incompleteItem.SellerName,
                incompleteItem.Name);
        }

        var totalItems = validItems.Count;

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);
        var pagedItems = validItems
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new CatalogImportResultDto
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Processed = pagedItems.Count,
            InvalidItems = invalidItems.Count,
            ItemsWithMissingBrand = itemsWithMissingBrand.Count,
            CreatedProducts = 0,
            LinkedSellerProducts = 0,
            IgnoredDuplicates = 0,
            Items = pagedItems
        };
    }

    private static string Normalize(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }
}
