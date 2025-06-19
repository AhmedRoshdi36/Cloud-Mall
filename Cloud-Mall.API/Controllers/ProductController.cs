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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? brand, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] double? minRate, [FromQuery] double? maxRate, [FromQuery] string? category, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new Cloud_Mall.Application.Products.Query.GetAllProductsQuery.GetAllProductsQuery(
                name, brand, minPrice, maxPrice, minRate, maxRate, category, pageNumber, pageSize);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new Cloud_Mall.Application.Products.Query.GetProductByIdQuery.GetProductByIdQuery(id);
            var result = await mediator.Send(query);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("cart")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> AddToCart([FromBody] Cloud_Mall.Application.DTOs.Cart.AddProductToCartCommand command)
        {
            var result = await mediator.Send(command);
            if (!result)
                return BadRequest("Could not add product to cart.");
            return Ok();
        }

        [HttpGet("cart")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ShowAllCartProducts()
        {
            var result = await mediator.Send(new Cloud_Mall.Application.Cart.Query.ShowAllCartProductsQuery());
            return Ok(result);
        }

        [HttpPut("cart/quantity")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] Cloud_Mall.Application.DTOs.Cart.UpdateCartItemQuantityCommand command)
        {
            var result = await mediator.Send(command);
            if (!result)
                return BadRequest("Could not update quantity.");
            return Ok();
        }

        [HttpDelete("cart/product")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] Cloud_Mall.Application.DTOs.Cart.RemoveProductFromCartCommand command)
        {
            var result = await mediator.Send(command);
            if (!result)
                return BadRequest("Could not remove product from cart.");
            return Ok();
        }

        [HttpGet("cart/total")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CalculateCartTotal()
        {
            var result = await mediator.Send(new Cloud_Mall.Application.Cart.Query.CalculateCartTotalQuery());
            return Ok(result);
        }
    }
}
