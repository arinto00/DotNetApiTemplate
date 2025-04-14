using ApiTemplate.Application.Common.Interfaces;
using ApiTemplate.Infrastructure.Data;
using ApiTemplate.Infrastructure.Identity;
using ApiTemplate.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiTemplate.Infrastructure;

/// <summary>
/// Extension methods for setting up infrastructure services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure services to the DI container
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Application configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Register interfaces
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        
        // Register DbContext initializer
        services.AddScoped<ApplicationDbContextInitializer>();
        
        // Register identity services
        services.AddSingleton<JwtTokenGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        // Register HTTP context accessor
        services.AddHttpContextAccessor();
        
        return services;
    }
}