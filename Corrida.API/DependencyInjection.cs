using CorridaAPI.Data;
using CorridaAPI.Data.Contracts;
using CorridaAPI.Model.Authentication;
using CorridaAPI.Services;
using CorridaAPI.Services.Contracts;
using CorridaAPI.Services.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CorridaAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddScoped<ICorridaService, CorridaService>();
        services.AddScoped<ISemearUsuarioPadrao, SemearUsuarioPadrao>();

        return services;
    }
    public static IServiceCollection AddDatabaseDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CarridaDatabaseSettings>
                (configuration.GetSection("ConnectMongoDB"));

        services.AddDbContext<CorridaContext>(cfg =>
        {
            cfg.UseSqlite("Data Source=Data\\CorridaUsers.db");
        });

        return services;
    }
    public static IServiceCollection AddAuthDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<CorridaContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey"]))
            };
        });

        services.AddAuthorization(auth => {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                                         .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                         .RequireAuthenticatedUser()
                                         .Build()
            );
        });

        return services;
    }
}
