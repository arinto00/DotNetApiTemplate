using System;

namespace ApiTemplate.Domain.Common;

/// <summary>
/// Base class for all entities
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; protected set; }
}