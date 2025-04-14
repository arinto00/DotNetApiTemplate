using ApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTemplate.Application.Common.Interfaces;

/// <summary>
/// Interface for the application database context
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// DbSet for Sample entities
    /// </summary>
    DbSet<Sample> Samples { get; }
    
    /// <summary>
    /// Saves all changes made in this context to the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected rows</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}