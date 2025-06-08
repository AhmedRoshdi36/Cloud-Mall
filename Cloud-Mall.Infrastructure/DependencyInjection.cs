using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Cloud_Mall.Infrastructure.Services.GoverningLocationService;
using Cloud_Mall.Infrastructure.Services.IdentityService;
using Cloud_Mall.Infrastructure.Services.StoreCategoryService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cloud_Mall.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
       this IServiceCollection services,
       IConfiguration configuration)
        {
            // 1. Register your concrete ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("conn1")
                ));

            // 2. Configure ASP.NET Core Identity
            // This is where IUserStore and IRoleStore are registered via .AddEntityFrameworkStores
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>() // <-- This line is crucial.
                                                              // It tells Identity to use ApplicationDbContext
                                                              // to store user and role information.
            .AddDefaultTokenProviders();

            // 3. Register your custom IIdentityService implementation
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IStoreCategoryRepository, StoreCategoryRepository>();
            services.AddScoped<IGoverningLocationRepository, GoverningLocationRepository>();

            // Add other infrastructure services if any

            return services;
        }
    }
}
