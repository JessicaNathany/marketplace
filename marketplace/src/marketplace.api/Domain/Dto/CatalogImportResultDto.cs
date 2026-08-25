namespace marketplace.api.Domain.Dto;

public class CatalogImportResultDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int Processed { get; set; }
    public int InvalidItems { get; set; }
    public int ItemsWithMissingBrand { get; set; }
    public int CreatedProducts { get; set; }
    public int LinkedSellerProducts { get; set; }
    public int IgnoredDuplicates { get; set; }
    public IReadOnlyCollection<ImportedSellerProductDto> Items { get; set; } = [];
}
