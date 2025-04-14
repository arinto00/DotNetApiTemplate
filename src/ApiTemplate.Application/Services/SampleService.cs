using ApiTemplate.Application.Common.Interfaces;
using ApiTemplate.Application.Common.Models;
using ApiTemplate.Application.Interfaces;
using ApiTemplate.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTemplate.Application.Services;

/// <summary>
/// Implementation of ISampleService
/// </summary>
public class SampleService : ISampleService
{
    private readonly IRepository<Sample> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<SampleService> _logger;

    /// <summary>
    /// Creates a new instance of SampleService
    /// </summary>
    /// <param name="repository">Sample repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="logger">Logger</param>
    public SampleService(
        IRepository<Sample> repository,
        IMapper mapper,
        ILogger<SampleService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<SampleDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (entity == null)
        {
            _logger.LogWarning("Sample with ID {SampleId} not found", id);
            return ApiResponse<SampleDto>.Failure("Sample not found");
        }

        return ApiResponse<SampleDto>.Success(_mapper.Map<SampleDto>(entity));
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<IEnumerable<SampleDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return ApiResponse<IEnumerable<SampleDto>>.Success(
            _mapper.Map<IEnumerable<SampleDto>>(entities));
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<SampleDto>> CreateAsync(CreateSampleDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = new Sample(createDto.Name, createDto.Description);
        
        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Created new sample with ID {SampleId}", entity.Id);
        
        return ApiResponse<SampleDto>.Success(_mapper.Map<SampleDto>(entity));
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<SampleDto>> UpdateAsync(Guid id, UpdateSampleDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (entity == null)
        {
            _logger.LogWarning("Sample with ID {SampleId} not found for update", id);
            return ApiResponse<SampleDto>.Failure("Sample not found");
        }

        entity.Update(updateDto.Name, updateDto.Description);
        
        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Updated sample with ID {SampleId}", id);
        
        return ApiResponse<SampleDto>.Success(_mapper.Map<SampleDto>(entity));
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (entity == null)
        {
            _logger.LogWarning("Sample with ID {SampleId} not found for deletion", id);
            return ApiResponse<bool>.Failure("Sample not found");
        }

        _repository.Delete(entity);
        await _repository.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Deleted sample with ID {SampleId}", id);
        
        return ApiResponse<bool>.Success(true);
    }
}