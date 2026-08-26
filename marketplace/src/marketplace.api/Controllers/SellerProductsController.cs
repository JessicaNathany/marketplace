using marketplace.api.Domain.Dto;
using marketplace.api.Domain.Entities;
using marketplace.api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace marketplace.api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/seller-product")]
    [Tags("Seller Product")]
    public class SellerProductsController(ISellerProductService service) : ControllerBase
    {
        [HttpGet()]
        [ProducesResponseType(typeof(SellerProduct), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CatalogImportResultDto>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
        {
            if (page < 1 || pageSize < 1 || pageSize > 200)
            {
                return BadRequest("Invalid pagination. Use page >= 1 and pageSize between 1 and 200.");
            }

            var result = await service.GetAllAsync(page, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpGet("by-seller")]
        [ProducesResponseType(typeof(SellerProduct), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CatalogImportResultDto>> GetBySellerName(
        [FromQuery] string sellerName,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
        {
            if (page < 1 || pageSize < 1 || pageSize > 200)
            {
                return BadRequest("Invalid pagination. Use page >= 1 and pageSize between 1 and 200.");
            }

            var result = await service.GetBySellerNameAsync(sellerName, page, pageSize, cancellationToken);
            return Ok(result);
        }   
    }
}
