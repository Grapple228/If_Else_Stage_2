using Database;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;

namespace WebApi.Helpers;

internal static class ConfigurationHelper
{
    internal static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    internal static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountsService, AccountsService>();
        services.AddScoped<IAnimalsService, AnimalsService>();
        services.AddScoped<IAreasService, AreasService>();
        services.AddScoped<ILocationsService, LocationsService>();
        services.AddScoped<ITypesService, TypesService>();
        services.AddScoped<IVisitedLocationService, VisitedLocationService>();
        return services;
    }
}