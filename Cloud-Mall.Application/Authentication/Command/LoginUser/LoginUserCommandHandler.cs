using Cloud_Mall.Application.DTOs.Auth;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Authentication.Commands.LoginUser
{
    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResponse<AuthenticationResult>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(IIdentityService identityService, ITokenGenerator jwtTokenGenerator)
        {
            _identityService = identityService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ApiResponse<AuthenticationResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the user by email
            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return ApiResponse<AuthenticationResult>.Failure("Invalid email or password");
            }

            // 2. Check the password
            var isPasswordValid = await _identityService.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return ApiResponse<AuthenticationResult>.Failure("Invalid email or password");
            }

            // 3. Get user roles
            var roles = await _identityService.GetRolesAsync(user);

            // 4. Generate the token
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            // 5. Return the result
            var auth = new AuthenticationResult
            {
                Token = token,
                UserId = user.Id
            };
            return ApiResponse<AuthenticationResult>.SuccessResult(auth);

        }
    }
}