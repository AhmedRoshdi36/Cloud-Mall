using Cloud_Mall.Application.DTOs.Auth;
using MediatR;

namespace Cloud_Mall.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<ApiResponse<AuthenticationResult>>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
