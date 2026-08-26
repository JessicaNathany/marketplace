using marketplace.api.Domain.Dto;
using marketplace.api.Domain.Entities;
using marketplace.api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace marketplace.api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/products")]
[Tags("Products")]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Product>>> GetProducts(
        [FromQuery] string? name,
        [FromQuery] string? brand,
        [FromQuery] string? category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || pageSize < 1 || pageSize > 200)
        {
            return BadRequest("Invalid pagination. Use page >= 1 and pageSize between 1 and 200.");
        }

        var products = await service.GetProductsAsync(name, brand, category, page, pageSize, cancellationToken);
        return Ok(products);
    }

    [HttpPost("import")]
    [ProducesResponseType(typeof(CatalogImportResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CatalogImportResultDto>> ProcessCatalogAddProduct(CancellationToken cancellationToken = default)
    {
        var result = await service.ProcessCatalogAddProduct(cancellationToken);
        return Ok(result);
    }
}
