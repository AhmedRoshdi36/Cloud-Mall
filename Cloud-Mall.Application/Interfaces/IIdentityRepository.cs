using Cloud_Mall.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IIdentityRepository
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<ApplicationUser?> FindByIdAsync(string email);
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<bool> DeleteAdminByIdAsync(string AdminId);

    }
}