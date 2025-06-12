using Cloud_Mall.Application.Authentication.Commands.LoginUser;
using Cloud_Mall.Application.Authentication.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IMediator mediator;

        public VendorController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            Console.WriteLine(command.Name);
            var result = await mediator.Send(command);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await mediator.Send(command);

            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
