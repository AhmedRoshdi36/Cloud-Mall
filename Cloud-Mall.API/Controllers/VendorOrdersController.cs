using Cloud_Mall.Application.Orders.Commands.UpdateStatus;
using Cloud_Mall.Application.Orders.Queries.GetForVendor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [Route("api/vendor/orders")]
    [ApiController]
    [Authorize(Roles = "Vendor")]
    public class VendorOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a list of all orders across ALL stores owned by the current vendor.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersForVendor()
        {
            var query = new GetAllOrdersForStoreQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a list of all orders for a SPECIFIC store owned by the current vendor.
        /// </summary>
        [HttpGet("store/{storeId}")]
        public async Task<IActionResult> GetAllOrdersForStore(int storeId)
        {
            var query = new GetAllOrdersForStoreQuery { StoreId = storeId };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Gets a single order by its ID, confirming the vendor owns it.
        /// </summary>
        //[HttpGet("{orderId}")]
        //public async Task<IActionResult> GetVendorOrderById(int orderId)
        //{
        //    var query = new GetVendorOrderByIdQuery { OrderId = orderId };
        //    var result = await _mediator.Send(query);
        //    return result.Success ? Ok(result) : NotFound(result);
        //}



        /// <summary>
        /// Updates the status of a specific vendor order (e.g., to 'Shipped' or 'Fulfilled').
        /// </summary>
        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusCommand command)
        {
            if (orderId != command.OrderId)
            {
                return BadRequest("Order ID in the URL and request body must match.");
            }

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                // This could be NotFound or BadRequest depending on the failure reason.
                return BadRequest(result);
            }

            // Returns a 204 No Content status, indicating success without returning data.
            return NoContent();
        }

    }
}