using System;

namespace ApiTemplate.Domain.Common;

/// <summary>
/// Base class for entities that require auditing information
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// User who created the entity
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Date and time when the entity was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }

    /// <summary>
    /// User who last modified the entity
    /// </summary>
    public string? LastModifiedBy { get; set; }
}