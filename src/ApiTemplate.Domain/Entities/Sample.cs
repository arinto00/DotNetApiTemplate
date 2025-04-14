using ApiTemplate.Domain.Common;
using ApiTemplate.Domain.Enums;

namespace ApiTemplate.Domain.Entities;

/// <summary>
/// Sample entity for demonstration purposes
/// </summary>
public class Sample : AuditableEntity
{
    /// <summary>
    /// Name of the sample
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    
    /// <summary>
    /// Description of the sample
    /// </summary>
    public string Description { get; private set; } = string.Empty;
    
    /// <summary>
    /// Current status of the sample
    /// </summary>
    public SampleStatus Status { get; private set; }

    // Private constructor for EF Core
    private Sample() { }

    /// <summary>
    /// Creates a new Sample entity
    /// </summary>
    /// <param name="name">Name of the sample</param>
    /// <param name="description">Description of the sample</param>
    public Sample(string name, string description)
    {
        Name = name;
        Description = description;
        Status = SampleStatus.Active;
    }

    /// <summary>
    /// Updates the sample information
    /// </summary>
    /// <param name="name">New name</param>
    /// <param name="description">New description</param>
    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Activates the sample
    /// </summary>
    public void Activate()
    {
        Status = SampleStatus.Active;
    }

    /// <summary>
    /// Deactivates the sample
    /// </summary>
    public void Deactivate()
    {
        Status = SampleStatus.Inactive;
    }

    /// <summary>
    /// Sets the sample status to pending
    /// </summary>
    public void MarkAsPending()
    {
        Status = SampleStatus.Pending;
    }
}