using ApiTemplate.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTemplate.Application.Interfaces;

/// <summary>
/// DTOs for Sample entities
/// </summary>
public record SampleDto(Guid Id, string Name, string Description, DateTime CreatedAt);
public record CreateSampleDto(string Name, string Description);
public record UpdateSampleDto(string Name, string Description);

/// <summary>
/// Service for managing Sample entities
/// </summary>
public interface ISampleService
{
    /// <summary>
    /// Gets a sample by ID
    /// </summary>
    /// <param name="id">Sample ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Sample data</returns>
    Task<ApiResponse<SampleDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all samples
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of samples</returns>
    Task<ApiResponse<IEnumerable<SampleDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new sample
    /// </summary>
    /// <param name="createDto">Sample data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created sample</returns>
    Task<ApiResponse<SampleDto>> CreateAsync(CreateSampleDto createDto, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing sample
    /// </summary>
    /// <param name="id">Sample ID</param>
    /// <param name="updateDto">Updated sample data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated sample</returns>
    Task<ApiResponse<SampleDto>> UpdateAsync(Guid id, UpdateSampleDto updateDto, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a sample
    /// </summary>
    /// <param name="id">Sample ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or failure</returns>
    Task<ApiResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}