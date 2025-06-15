using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Store.Command.AddStoreAddresses;
using Cloud_Mall.Application.Store.Command.CreateStore;
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

        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> CreateStore([FromForm] CreateStoreCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        [HttpPost("addresses/{storeId}")]
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
            {
                return BadRequest(result);
            }

            return Created("", result);
        }
    }
}
