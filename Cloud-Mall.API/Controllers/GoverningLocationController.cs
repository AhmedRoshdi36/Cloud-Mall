using Cloud_Mall.Application.DTOs.GoverningLocation;
using Cloud_Mall.Application.GoverningLocations.Command.CreateGoverningLocation;
using Cloud_Mall.Application.GoverningLocations.Query.GetAllGoverningLocations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoverningLocationController : ControllerBase
    {
        private readonly IMediator mediator;
        public GoverningLocationController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await mediator.Send(new GetAllGoverningLocationsQuery());
            return Ok(locations);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGoverningLocationsDTO dto)
        {
            var command = await mediator.Send(new CreateGoverningLocationCommand()
            {
                Name = dto.Name,
                Region = dto.Region,
            });
            return Created("", command);
        }
    }
}
