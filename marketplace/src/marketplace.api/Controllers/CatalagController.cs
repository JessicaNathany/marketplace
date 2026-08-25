using marketplace.api.Domain.Dto;
using marketplace.api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace marketplace.api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/catalog")]
[Tags("Catalog")]
public class CatalogController(ICatalogService service) : ControllerBase
{
    [HttpPost("import")]
    [ProducesResponseType(typeof(CatalogImportResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CatalogImportResultDto>> ImportCatalog(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || pageSize < 1 || pageSize > 200)
        {
            return BadRequest("Invalid pagination. Use page >= 1 and pageSize between 1 and 200.");
        }

        var result = await service.ImportCatalogAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

}
