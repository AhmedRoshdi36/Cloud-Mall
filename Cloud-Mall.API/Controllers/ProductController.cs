using Cloud_Mall.Application.Products.Command.CreateProduct;
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
    }
}
