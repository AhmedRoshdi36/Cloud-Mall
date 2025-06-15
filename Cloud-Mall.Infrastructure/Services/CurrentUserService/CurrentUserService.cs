using System.Security.Claims;
using Cloud_Mall.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Cloud_Mall.Infrastructure.Services.CurrentUserService
{
    internal class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("id");

        public IEnumerable<Claim>? GetUserClaims()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims;
        }
    }
}
