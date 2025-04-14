using ApiTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTemplate.API.Controllers;

/// <summary>
/// API controller for managing Sample entities
/// </summary>
public class SamplesController : BaseApiController
{
    private readonly ISampleService _sampleService;

    /// <summary>
    /// Creates a new instance of SamplesController
    /// </summary>
    /// <param name="sampleService">Sample service</param>
    public SamplesController(ISampleService sampleService)
    {
        _sampleService = sampleService;
    }

    /// <summary>
    /// Gets all samples
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of samples</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sampleService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a sample by ID
    /// </summary>
    /// <param name="id">Sample ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Sample</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sampleService.GetByIdAsync(id, cancellationToken);
        
        if (!result.Succeeded)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Creates a new sample
    /// </summary>
    /// <param name="createDto">Sample data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created sample</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateSampleDto createDto, CancellationToken cancellationToken)
    {
        var result = await _sampleService.CreateAsync(createDto, cancellationToken);
        
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Updates an existing sample
    /// </summary>
    /// <param name="id">Sample ID</param>
    /// <param name="updateDto">Updated sample data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated sample</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateSampleDto updateDto, CancellationToken cancellationToken)
    {
        var result = await _sampleService.UpdateAsync(id, updateDto, cancellationToken);
        
        if (!result.Succeeded)
        {
            return result.Error!.Contains("not found") 
                ? NotFound(result) 
                : BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Deletes a sample
    /// </summary>
    /// <param name="id">Sample ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sampleService.DeleteAsync(id, cancellationToken);
        
        if (!result.Succeeded)
        {
            return NotFound(result);
        }
        
        return NoContent();
    }
}