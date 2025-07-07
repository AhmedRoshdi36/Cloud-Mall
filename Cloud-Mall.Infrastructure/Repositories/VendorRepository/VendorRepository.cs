

using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories.VendorRepository;

internal class VendorRepository(ApplicationDbContext context) : IVendorRepository
{
    private readonly ApplicationDbContext context = context;

    public async Task<List<ApplicationUser>> GetAllVendorsAsync()
    {
      return await (
       from u in context.Users
       join ur in context.UserRoles on u.Id equals ur.UserId
       join r in context.Roles on ur.RoleId equals r.Id
       where r.Name == "Vendor"
       select u
       ).ToListAsync();
    }

}

