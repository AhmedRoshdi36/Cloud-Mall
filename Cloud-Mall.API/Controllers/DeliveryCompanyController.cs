using Cloud_Mall.Application.Common;
using Cloud_Mall.Application.DeliveryCompany.Command.RegisterDeliveryCompany;
using Cloud_Mall.Application.DeliveryCompany.Query.GetUndeliveredOrders;
using Cloud_Mall.Application.DeliveryCompany.Query.GetVendorContactInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryCompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryCompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] RegisterDeliveryCompanyCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok((result, "Delivery company registered successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure("An error occurred while registering the delivery company"));
            }
        }

        [HttpGet("undelivered-orders")]
        [Authorize(Roles = "Delivery")]
        public async Task<ActionResult<ApiResponse<object>>> GetUndeliveredOrders()
        {
            try
            {
                var query = new GetUndeliveredOrdersQuery();
                var result = await _mediator.Send(query);
                return Ok((result, "Undelivered orders retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure("An error occurred while retrieving undelivered orders"));
            }
        }

        [HttpGet("vendor-contact-info/{customerOrderId}")]
        [Authorize(Roles = "Delivery")]
        public async Task<ActionResult<ApiResponse<object>>> GetVendorContactInfo(int customerOrderId)
        {
            try
            {
                var query = new GetVendorContactInfoQuery { CustomerOrderID = customerOrderId };
                var result = await _mediator.Send(query);
                return Ok((result, "Vendor contact information retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure("An error occurred while retrieving vendor contact information"));
            }
        }
    }
} 