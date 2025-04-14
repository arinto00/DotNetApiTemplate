using ApiTemplate.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTemplate.Application.Common.Interfaces;

/// <summary>
/// Generic repository interface for CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Gets an entity by its ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity or null if not found</returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of entities</returns>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Finds entities based on a predicate
    /// </summary>
    /// <param name="predicate">Search predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of matching entities</returns>
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds a new entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Added entity</returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">Entity to update</param>
    void Update(T entity);
    
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="entity">Entity to delete</param>
    void Delete(T entity);
    
    /// <summary>
    /// Saves all changes made in this repository
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected rows</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}