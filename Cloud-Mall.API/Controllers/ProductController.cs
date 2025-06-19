using Cloud_Mall.Application.Products.Command.CreateProduct;
using Cloud_Mall.Application.Products.Query.GetAllProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("vendor")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        [HttpGet("vendor/{storeId:int}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> GetAllForStore([FromRoute] int storeId)
        {
            var query = new GetAllProductsForStoreQuery()
            {
                StoreId = storeId
            };
            var result = await mediator.Send(query);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }
    }
}
