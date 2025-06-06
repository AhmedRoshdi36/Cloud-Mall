using Cloud_Mall.Application.DTOs.Auth;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string name, string email, string password, string role);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}
