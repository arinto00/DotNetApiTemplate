namespace ApiTemplate.Domain.Enums;

/// <summary>
/// Status options for the Sample entity
/// </summary>
public enum SampleStatus
{
    /// <summary>
    /// Item is active and available
    /// </summary>
    Active = 1,
    
    /// <summary>
    /// Item is inactive or archived
    /// </summary>
    Inactive = 2,
    
    /// <summary>
    /// Item is pending approval or processing
    /// </summary>
    Pending = 3
}