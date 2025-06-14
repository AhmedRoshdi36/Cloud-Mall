using Cloud_Mall.Application.DTOs.Auth;
using MediatR;

namespace Cloud_Mall.Application.Authentication.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<Result<AuthenticationResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
