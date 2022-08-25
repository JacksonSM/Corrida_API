﻿using CorridaAPI.Authentication.Identity;
using CorridaAPI.Data;
using CorridaAPI.Services;
using CorridaAPI.Services.Contracts;
using CorridaAPI.Services.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CorridaAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddScoped<ICorridaService, CorridaService>();


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
    public static IServiceCollection AddAuthDependency(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<CorridaContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}