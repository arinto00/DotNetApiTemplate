using ApiTemplate.Application.Interfaces;
using ApiTemplate.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApiTemplate.Application;

/// <summary>
/// Extension methods for setting up application services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds application services to the DI container
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Register services
        services.AddScoped<ISampleService, SampleService>();
        
        return services;
    }
}