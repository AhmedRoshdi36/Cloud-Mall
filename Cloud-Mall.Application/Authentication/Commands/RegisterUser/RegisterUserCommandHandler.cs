using Cloud_Mall.Application.DTOs.Auth;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthenticationResult>
    {
        private readonly IIdentityService identityService;

        public RegisterUserCommandHandler(IIdentityService _identityService)
        {
            identityService = _identityService;
        }

        public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await identityService.RegisterAsync(request.Name, request.Email, request.Password, request.Role);
        }
    }
}
