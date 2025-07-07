using Cloud_Mall.Application.Orders.Commands.CreateOrderFromCart;
using Cloud_Mall.Application.Orders.Queries.GetForClient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize(Roles = "Client")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new order from the contents of the client's current shopping cart.
        /// </summary>
        [HttpPost("checkout")]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderFromCartCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            // Returns a 201 Created status with a link to the newly created customer order.
            return CreatedAtAction(nameof(GetCustomerOrderById), new { orderId = result.Data }, result);
        }

        /// <summary>
        /// Gets a list of all orders placed by the current client.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllCustomerOrders()
        {
            var query = new GetAllCustomerOrdersQuery();
            var result = await _mediator.Send(query);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Gets the details of a specific order, including all its sub-orders for each vendor.
        /// </summary>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetCustomerOrderById(int orderId)
        {
            var query = new GetCustomerOrderByIdQuery { OrderId = orderId };
            var result = await _mediator.Send(query);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}