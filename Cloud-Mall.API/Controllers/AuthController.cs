using Cloud_Mall.Application.Admin.Users.Query.GetAllAdminsBySuperAdminQuery;
using Cloud_Mall.Application.Admin.Users.Query.GetAllClientsByAdminQuery;
using Cloud_Mall.Application.Admin.Users.Query.GetAllVendorsByAdminQuery;
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


        [HttpPost("SuperAdmin/CreateAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        [Tags("AdminSuper - Admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterUserDTO request)
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

        [HttpGet("SuperAdmin/GetAllAdminsBySuperAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        [Tags("AdminSuper - Admin")]
        public async Task<IActionResult> GetAllAdminsBySuperAdmin()
        {
            var query = new GetAllAdminsBySuperAdminQuery();
            var result = await mediator.Send(query);
            if (!result.Success)
                return BadRequest(result);
            return Created("", result);
        }

        [HttpGet("Admin/GetAllClientsByAdmin")]
        [Authorize(Roles = "SuperAdmin,admin")]
        [Tags("Admin - Client")]
        public async Task<IActionResult> GetAllClientsByAdmin()
        {
            var query = new GetAllClientsByAdminQuery();
            var result = await mediator.Send(query);
            if (!result.Success)
                return BadRequest(result);
            return Created("", result);
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
