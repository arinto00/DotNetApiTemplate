using System;

namespace ApiTemplate.Domain.Exceptions;

/// <summary>
/// Base exception for all domain exceptions
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Creates a new instance of the DomainException class
    /// </summary>
    public DomainException() { }

    /// <summary>
    /// Creates a new instance of the DomainException class with a message
    /// </summary>
    /// <param name="message">Exception message</param>
    public DomainException(string message) : base(message) { }

    /// <summary>
    /// Creates a new instance of the DomainException class with a message and inner exception
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public DomainException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when an entity is not found
/// </summary>
public class EntityNotFoundException : DomainException
{
    /// <summary>
    /// Creates a new instance of the EntityNotFoundException class
    /// </summary>
    /// <param name="entityName">Name of the entity</param>
    /// <param name="id">ID of the entity</param>
    public EntityNotFoundException(string entityName, object id) 
        : base($"Entity {entityName} with ID {id} was not found.") { }
}

/// <summary>
/// Exception thrown when an invalid operation is attempted on an entity
/// </summary>
public class InvalidEntityOperationException : DomainException
{
    /// <summary>
    /// Creates a new instance of the InvalidEntityOperationException class
    /// </summary>
    /// <param name="message">Exception message</param>
    public InvalidEntityOperationException(string message) : base(message) { }
}