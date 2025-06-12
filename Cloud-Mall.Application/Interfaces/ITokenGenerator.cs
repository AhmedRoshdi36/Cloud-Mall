using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
