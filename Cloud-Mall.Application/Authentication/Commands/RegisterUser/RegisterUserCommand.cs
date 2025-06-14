using System.ComponentModel.DataAnnotations;
using Cloud_Mall.Application.DTOs.Auth;
using MediatR;

namespace Cloud_Mall.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<AuthenticationResult>
    {
        public string Name { get; set; }=null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }= null!;
        public string Role { get; set; } = null!;
    }
}
