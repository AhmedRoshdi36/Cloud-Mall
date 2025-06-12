using System.ComponentModel.DataAnnotations;
using Cloud_Mall.Application.DTOs.Auth;
using MediatR;

namespace Cloud_Mall.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<AuthenticationResult>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }
}
