using Cloud_Mall.Application.DTOs.Auth;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthenticationResult>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenGenerator _jwtTokenGenerator;

        public RegisterUserCommandHandler(IIdentityService identityService, ITokenGenerator jwtTokenGenerator)
        {
            _identityService = identityService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Business Logic: Check if user already exists
            var existingUser = await _identityService.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthenticationResult { Errors = new[] { "User with this email already exists." } };
            }

            // 2. Create the user entity
            var newUser = new ApplicationUser
            {
                Name = request.Name,
                Email = request.Email,
                UserName = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            // 3. Persist the user
            var createdUserResult = await _identityService.CreateUserAsync(newUser, request.Password);
            if (!createdUserResult.Succeeded)
            {
                return new AuthenticationResult { Errors = createdUserResult.Errors.Select(e => e.Description) };
            }

            // 4. Assign the role
            var roleResult = await _identityService.AddToRoleAsync(newUser, request.Role);
            if (!roleResult.Succeeded)
            {
                // In a real app, you might want to delete the user here (rollback)
                return new AuthenticationResult { Errors = roleResult.Errors.Select(e => e.Description) };
            }

            // 5. Get roles for token generation
            var roles = await _identityService.GetRolesAsync(newUser);

            // 6. Generate the token
            var token = _jwtTokenGenerator.GenerateToken(newUser, roles);

            // 7. Return the successful result DTO
            return new AuthenticationResult
            {
                Succeeded = true,
                Token = token,
                UserId = newUser.Id
            };
        }
    }
}