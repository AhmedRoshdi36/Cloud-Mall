using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Application.Interfaces.Repositories;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Cloud_Mall.Infrastructure.Repositories;
using Cloud_Mall.Infrastructure.Repositories.CartRepository;
using Cloud_Mall.Infrastructure.Repositories.DeliveryCompanyRepository;
using Cloud_Mall.Infrastructure.Repositories.DeliveryOfferRepository;
using Cloud_Mall.Infrastructure.Repositories.ProductRepository;
using Cloud_Mall.Infrastructure.Repositories.StoreRepository;
using Cloud_Mall.Infrastructure.Services.CurrentUserService;
using Cloud_Mall.Infrastructure.Services.FileService;
using Cloud_Mall.Infrastructure.Services.JwtTokenGenerator;
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IStoreCategoryRepository, StoreCategoryRepository>();
            services.AddScoped<IGoverningLocationRepository, GoverningLocationRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IDeliveryCompanyRepository, DeliveryCompanyRepository>();
            services.AddScoped<IDeliveryOfferRepository, DeliveryOfferRepository>();

            // Add other infrastructure services if any

            return services;
        }
    }
}
