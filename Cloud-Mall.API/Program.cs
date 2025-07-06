using System.Text;
using Cloud_Mall.Application;
using Cloud_Mall.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
namespace Cloud_Mall.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();


        //builder.Services.AddControllers();
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false; // In development; set to true in production
            var secret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("JWT Secret is not configured.");
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
            };
        });
        builder.Services.AddAuthorization();

        //builder.Services.AddCors(options =>
        //    {
        //        options.AddPolicy("AllowAll",
        //            policy =>
        //            {
        //                policy.AllowAnyOrigin()
        //                      .AllowAnyHeader()
        //                      .AllowAnyMethod();
        //            });
        //    });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowCloudMallFront", policy =>
            {
                policy.WithOrigins(
                        "http://cloudmallfront.runasp.net",
                        "https://cloudmallfront.runasp.net",
                        "http://localhost:4200",
                        "https://localhost:4200"
                        )
                      .AllowCredentials()
                      .WithHeaders("Content-Type", "Authorization")
                      .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
            });
        });


        //Access-Control-Allow-Origin: http://cloudmallfront.runasp.net
        //Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS
        //Access-Control-Allow-Headers: Content-Type, Authorization

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "CloudMall API", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
        });




        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //using var scope = app.Services.CreateScope();
            //await DataGenerator.SeedAsync(scope.ServiceProvider);
        }
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseCors("AllowCloudMallFront");

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
