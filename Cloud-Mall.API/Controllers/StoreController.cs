﻿using Cloud_Mall.Application.Admin.StoreManagement.Command.DeleteStoreByAdmin;
using Cloud_Mall.Application.Admin.StoreManagement.Command.DisableStoreByAdmin;
using Cloud_Mall.Application.Admin.StoreManagement.Command.EnableStoreByAdmin;
using Cloud_Mall.Application.Admin.StoreManagement.Query.GetAllStoresByAdmin;
using Cloud_Mall.Application.DTOs.ProductCategory;
using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.ProductCategories.Command.CreateProductCategory;
using Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategories;
using Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategoriesClient;
using Cloud_Mall.Application.Stores.Command.AddStoreAddresses;
using Cloud_Mall.Application.Stores.Command.CreateStore;
using Cloud_Mall.Application.Stores.Query.GetAllVendorStoresQuery;
using Cloud_Mall.Application.Stores.Query.GetStoreByIdQuery;
using Cloud_Mall.Application.Stores.Query.GetVendorStoreById;
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
            return Ok(result);
        }

        [HttpGet("productcategory/{storeId:int}")]
        public async Task<IActionResult> GetAllStoreCategoriesClient([FromRoute] int storeId)
        {
            var query = new GetAllProductCategoriesClientQuery() { storeId = storeId };
            var result = await mediator.Send(query);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("vendor/getallstores")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> GetAllVendorStores()
        {
            var query = new GetAllVendorStoresQuery();
            var result = await mediator.Send(query);
            if (!result.Success)
                return BadRequest(result);
            return Created("", result);
        }

        [HttpGet("Vendor/GetOneStore/{storeId:int}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> GetOneStoreByVendor(int storeId)
        {
            var query = new GetVendorStoreByIdQuery();
            query.StoreId = storeId;
            var result = await mediator.Send(query);
            if (!result.Success)
                return BadRequest(result);
            return Created("", result);
        }


        //[HttpGet("get-all-stores")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetAllStores(string? categoryName = null, int pageNumber = 1,
        //    int pageSize = 10)
        //{
        //    var query = new Cloud_Mall.Application.Stores.Query.GetAllStoresQuery.GetAllStoresQuery(categoryName, pageNumber, pageSize);
        //    var result = await mediator.Send(query);
        //    if (result == null)
        //        return NotFound("No stores found");
        //    return Ok(result);

        //}
        [HttpGet("get-all-stores")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllStoresWithFilteration(
                [FromQuery] int? CategoryId,
                [FromQuery] int? GoverningLocationId,
                [FromQuery] string? StreetAddress,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10)
        {
            var result = await mediator.Send(new GetAllStoresWithFilterationQuery
            {
                CategoryId = CategoryId,
                GoverningLocationId = GoverningLocationId,
                StreetAddress = StreetAddress,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return Ok(result);
        }





        [HttpGet("get-store/{storeId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreById([FromRoute] int storeId)
        {
            var result = await mediator.Send(new GetStoreByIdQuery(storeId));
            if (result == null)
                return NotFound("Store not found");
            return Ok(result);
        }
        [HttpGet("Admin/GetAllStores")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Tags("Admin - Stores")]
        public async Task<IActionResult> GetAllStoresByADmin(string? categoryName = null, int pageNumber = 1,
            int pageSize = 10)
        {
            var query = new GetAllStoresByAdminQuery(categoryName, pageNumber, pageSize);
            var result = await mediator.Send(query);
            if (result == null)
                return NotFound("No stores found");
            return Created("", result);
        }

        [HttpDelete("Admin/DeleteStoreByAdmin/{storeId:int}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Tags("Admin - Stores")]
        public async Task<IActionResult> DeleteStoreByAdmin([FromRoute] int storeId)
        {
            var command = new DeleteStoreByAdminCommand(storeId);
            var result = await mediator.Send(command);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }
        [HttpPost("Admin/EnableStoreByAdmin/{storeId:int}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Tags("Admin - Stores")]
        public async Task<IActionResult> EnableStoreByAdmin([FromRoute] int storeId)
        {
            var command = new EnableStoreByAdminCommand(storeId);
            var result = await mediator.Send(command);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }

        [HttpPost("Admin/DisableStoreByAdmin/{storeId:int}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Tags("Admin - Stores")]
        public async Task<IActionResult> DisableStoreByAdmin([FromRoute] int storeId)
        {
            var command = new DisableStoreByAdminCommand(storeId);
            var result = await mediator.Send(command);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }
    }
}
