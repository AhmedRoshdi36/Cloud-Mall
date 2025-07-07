using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Orders.Commands.UpdateStatus;
using Cloud_Mall.Application.Orders.Queries.GetForVendor;
using Cloud_Mall.Domain.Enums;
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
            var query = new GetAllVendorOrdersQuery();
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
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO request)
        {
            if (!Enum.TryParse<VendorOrderStatus>(request.newStatus, true, out var parsedStatus))
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Errors = new() { $"The status '{request.newStatus}' is not a valid order status." }
                });
            }

            var command = new UpdateOrderStatusCommand
            {
                OrderId = orderId,
                NewStatus = parsedStatus
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

    }
}