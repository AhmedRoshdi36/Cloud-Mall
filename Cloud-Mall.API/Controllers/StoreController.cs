using Cloud_Mall.Application.DTOs.ProductCategory;
using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.ProductCategories.Command.CreateProductCategory;
using Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategories;
using Cloud_Mall.Application.Stores.Command.AddStoreAddresses;
using Cloud_Mall.Application.Stores.Command.CreateStore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IMediator mediator;
        public StoreController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("vendor")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> CreateStore([FromForm] CreateStoreCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.Success)
                return BadRequest(result);
            return Created("", result);
        }

        [HttpPost("vendor/addresses/{storeId}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> AddAddresses(int storeId, [FromBody] List<StoreAddressDTO> addressDtos)
        {
            var command = new AddStoreAddressesCommand
            {
                StoreId = storeId,
                Addresses = addressDtos
            };

            var result = await mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Created("", result);
        }

        [HttpPost("vendor/productcategory/{storeId:int}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> AddProductCategoryForStore([FromRoute] int storeId, [FromBody] CreateProductCategoryDTO request)
        {
            var command = new CreateProductCategoryCommand()
            {
                StoreID = storeId,
                Name = request.Name,
                Description = request.Description,
            };
            var result = await mediator.Send(command);
            if (!result.Success)
                return BadRequest(result);
            return Created("", result);
        }

        [HttpGet("vendor/productcategory/{storeId:int}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> GetAllStoreCategories([FromRoute] int storeId)
        {
            var query = new GetAllProductCategoriesQuery() { storeId = storeId };
            var result = await mediator.Send(query);
            if (!result.Success)
                return BadRequest(result);
            return Created("", result);
        }
        [HttpGet("get-all-stores")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllStores()
        {
            var query = new Cloud_Mall.Application.Stores.Query.GetAllStoresQuery.GetAllStoresQuery();
            var result = await mediator.Send(query);
            return Ok(result);

        }
    }
}
