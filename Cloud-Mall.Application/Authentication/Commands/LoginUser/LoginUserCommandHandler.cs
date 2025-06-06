using Cloud_Mall.Application.DTOs.Auth;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Authentication.Commands.LoginUser
{
    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthenticationResult>
    {
        private readonly IIdentityService identityService;
        public LoginUserCommandHandler(IIdentityService _identityService)
        {
            identityService = _identityService;
        }
        public async Task<AuthenticationResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await identityService.LoginAsync(request.Email, request.Password);
        }
    }
}
