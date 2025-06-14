using Cloud_Mall.Application.Authentication.Commands.LoginUser;
using Cloud_Mall.Application.Authentication.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        public AuthController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.Success)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
