using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cloud_Mall.Application.DTOs.Auth;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cloud_Mall.Infrastructure.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public IdentityService(
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager, IConfiguration _configuration)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            configuration = _configuration;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid email or password." } };
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid email or password." } };
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(string name, string email, string password, string role)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthenticationResult { Errors = new[] { "User with this email already exists." } };
            }

            var newUser = new ApplicationUser
            {
                Name = name, //
                Email = email,
                UserName = email, // IdentityUser requires UserName
                CreatedAt = DateTime.UtcNow //
            };

            var createdUser = await userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult { Errors = createdUser.Errors.Select(e => e.Description) };
            }

            // Add role to user - Ensure roles like "Vendor" and "Client" are created (e.g., in Program.cs or a seeder)
            var roleResult = await userManager.AddToRoleAsync(newUser, role);
            if (!roleResult.Succeeded)
            {
                // Handle role assignment failure (e.g., log, or return specific error)
                // For simplicity, we'll proceed but in a real app, you might want to roll back user creation or retry.
                return new AuthenticationResult { Errors = roleResult.Errors.Select(e => e.Description).Concat(new[] { "User created but failed to assign role." }) };
            }


            // Optionally: Generate token upon successful registration
            return await GenerateAuthenticationResultForUserAsync(newUser);
        }


        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"]); // Ensure JwtSettings:Secret is in appsettings.json

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("id", user.Id)
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(configuration["JwtSettings:TokenLifetime"])), // e.g., "JwtSettings:TokenLifetime": "01:00:00"
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = configuration["JwtSettings:Issuer"],
                Audience = configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Succeeded = true,
                Token = tokenHandler.WriteToken(token),
                UserId = user.Id
            };
        }
    }
}
