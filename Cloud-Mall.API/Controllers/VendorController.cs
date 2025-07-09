using Cloud_Mall.Application.Admin.Users.Query.GetAllVendorsByAdminQuery;
using Cloud_Mall.Application.Stores.Query.GetAllVendorStoresQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VendorController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("Admin/GetAllVendorsByAdmin")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Tags("Admin - Vendors")]
        public async Task<IActionResult> GetAllVendorsByAdmin()
        {
            var query = new GetAllVendorsByAdminQuery();
            var result = await _mediator.Send(query);
            if (!result.Success)
                return BadRequest(result);
            return Created("", result);
        }
        [HttpGet("admin/vendors/{vendorId}/stores")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Tags("Admin - Vendors")]
        public async Task<IActionResult> GetAllVendorStoresByAdmin([FromRoute] string vendorId)
        {
            if (string.IsNullOrWhiteSpace(vendorId))
                return BadRequest("Vendor ID must not be empty.");

            var query = new GetAllVendorStoresByAdminQuery(vendorId);
            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result); 
        }

    }
}
