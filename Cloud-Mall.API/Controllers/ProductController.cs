using Cloud_Mall.Application.Admin.ProductManagement.Command.DeleteProductByAdmin;
using Cloud_Mall.Application.Products.Command.CreateProduct;
using Cloud_Mall.Application.Products.Query.GetAllProducts;
using Cloud_Mall.Application.Products.Query.GetAllProductsQuery;
using Cloud_Mall.Application.Products.Query.GetProductForVendor;
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

        [HttpPost("vendor/{storeId:int}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand command, [FromRoute] int storeId)
        {
            command.StoreID = storeId;
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
            return Ok(result);

        }

        [HttpGet("vendor/getproductbyid/{productId:int}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> GetSingleProductForVendor([FromRoute] int productId)
        {
            var command = new GetSingleProductForVendorQuery()
            {
                ProductId = productId
            };
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }



        [HttpDelete("Admin/deleteproductByAdmin/{productId:int}")]
        [Authorize(Roles = "Admin")]
        [Tags("Admin - Products")]
        public async Task<IActionResult> DeleteProductByAdmin([FromRoute] int productId)
        {
            var command = new DeleteProductByAdminCommand(productId);
            var result = await mediator.Send(command);

            if (!result.Success)
                return NotFound(result); 

            return NoContent(); 
        }

        [HttpDelete("Admin/deleteProductsByAdmin/{storeId:int}")]
        [Authorize(Roles = "Admin")]
        [Tags("Admin - Products")]
        public async Task<IActionResult> DeleteProductsByAdmin([FromRoute] int storeId)
        {
            var command = new DeleteProductsByAdminCommand(storeId);
            var result = await mediator.Send(command);

            if (!result.Success)
                return NotFound(result);

            return Ok();

        }




        [HttpGet("{storeId:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int storeId, [FromQuery] GetAllProductsQuery query)
        {
            query.StoreId = storeId;
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
