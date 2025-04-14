using ApiTemplate.Application.Common.Interfaces;
using ApiTemplate.Domain.Common;
using ApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTemplate.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the application
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly DateTime _currentDateTime;

    /// <summary>
    /// Sample entities
    /// </summary>
    public DbSet<Sample> Samples => Set<Sample>();

    /// <summary>
    /// Creates a new instance of ApplicationDbContext
    /// </summary>
    /// <param name="options">DbContext options</param>
    /// <param name="currentUserService">Current user service</param>
    public ApplicationDbContext(
        DbContextOptions options,
        ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
        _currentDateTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Configures the model
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Saves all changes made in this context to the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected rows</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Set auditing properties
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.CreatedAt = _currentDateTime;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModifiedAt = _currentDateTime;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}