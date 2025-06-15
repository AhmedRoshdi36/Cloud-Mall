using System.Security.Claims;

namespace Cloud_Mall.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        IEnumerable<Claim>? GetUserClaims();
    }
}
