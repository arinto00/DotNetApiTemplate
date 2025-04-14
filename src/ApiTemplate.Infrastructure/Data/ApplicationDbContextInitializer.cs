using ApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiTemplate.Infrastructure.Data;

/// <summary>
/// Initializes and seeds the database
/// </summary>
public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Creates a new instance of ApplicationDbContextInitializer
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger</param>
    public ApplicationDbContextInitializer(
        ApplicationDbContext context,
        ILogger<ApplicationDbContextInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Initializes the database
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
            else
            {
                await _context.Database.EnsureCreatedAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    /// <summary>
    /// Attempts to seed the database with initial data
    /// </summary>
    private async Task TrySeedAsync()
    {
        // Seed Samples if no samples exist
        if (!await _context.Samples.AnyAsync())
        {
            _logger.LogInformation("Seeding sample data");
            
            _context.Samples.Add(new Sample("Sample 1", "This is the first sample"));
            _context.Samples.Add(new Sample("Sample 2", "This is the second sample"));
            
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Seeded sample data");
        }
    }
}