using Cloud_Mall.Application.Common;
using Cloud_Mall.Application.DeliveryOffer.Command.AcceptDeliveryOffer;
using Cloud_Mall.Application.DeliveryOffer.Command.CreateDeliveryOffer;
using Cloud_Mall.Application.DeliveryOffer.Command.RejectDeliveryOffer;
using Cloud_Mall.Application.DeliveryOffer.Query.GetDeliveryOffersForOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryOfferController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryOfferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Delivery")]
        public async Task<ActionResult<ApiResponse<object>>> CreateOffer([FromBody] CreateDeliveryOfferCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok((result, "Delivery offer created successfully"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.Failure(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure("An error occurred while creating the delivery offer"));
            }
        }

        [HttpGet("order/{customerOrderId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> GetOffersForOrder(int customerOrderId)
        {
            try
            {
                var query = new GetDeliveryOffersForOrderQuery { CustomerOrderID = customerOrderId };
                var result = await _mediator.Send(query);
                return Ok((result, "Delivery offers retrieved successfully"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure("An error occurred while retrieving delivery offers"));
            }
        }

        [HttpPost("{offerId}/accept")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> AcceptOffer(int offerId)
        {
            try
            {
                var command = new AcceptDeliveryOfferCommand { OfferID = offerId };
                var result = await _mediator.Send(command);
                return Ok((result, "Delivery offer accepted successfully"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.Failure(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure("An error occurred while accepting the delivery offer"));
            }
        }

        [HttpPost("{offerId}/reject")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> RejectOffer(int offerId)
        {
            try
            {
                var command = new RejectDeliveryOfferCommand { OfferID = offerId };
                var result = await _mediator.Send(command);
                return Ok((result, "Delivery offer rejected successfully"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.Failure(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure("An error occurred while rejecting the delivery offer"));
            }
        }
    }
} 