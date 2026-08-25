using marketplace.api.Domain.Dto;

namespace marketplace.api.Service.Interfaces;
public interface ICatalogService
{
    Task<CatalogImportResultDto> ImportCatalogAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
