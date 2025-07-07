using Cloud_Mall.Application.Authentication.Commands.LoginUser;
using Cloud_Mall.Application.Authentication.Commands.RegisterUser;
using Cloud_Mall.Application.DTOs.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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


        [HttpPost("Admin/register")]
        [Authorize(Roles = "SuperAdmin")]
        [Tags("Admin")]

        public async Task<IActionResult> AdminRegister([FromBody] RegisterUserDTO request)
        {
            var command = new RegisterUserCommand()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                Role = "Admin",
            };
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("vendor/register")]
        public async Task<IActionResult> VendorRegister([FromBody] RegisterUserDTO request)
        {
            var command = new RegisterUserCommand()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                Role = "Vendor",
            };
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("client/register")]
        public async Task<IActionResult> ClientRegister([FromBody] RegisterUserDTO request)
        {
            var command = new RegisterUserCommand()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                Role = "Client",
            };
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("delivery/register")]
        public async Task<IActionResult> DeliveryRegister([FromBody] RegisterUserDTO request)
        {
            var command = new RegisterUserCommand()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                Role = "Delivery",
            };
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
