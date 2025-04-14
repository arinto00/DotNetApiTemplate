namespace ApiTemplate.Application.Common.Models;

/// <summary>
/// Standard API response model
/// </summary>
/// <typeparam name="T">Type of the data returned</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Whether the request was successful
    /// </summary>
    public bool Succeeded { get; private set; }
    
    /// <summary>
    /// The data returned by the API
    /// </summary>
    public T? Data { get; private set; }
    
    /// <summary>
    /// Error message if the request failed
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Private constructor to enforce use of factory methods
    /// </summary>
    private ApiResponse() { }

    /// <summary>
    /// Creates a successful response with data
    /// </summary>
    /// <param name="data">Response data</param>
    /// <returns>ApiResponse instance</returns>
    public static ApiResponse<T> Success(T data)
    {
        return new ApiResponse<T> { Succeeded = true, Data = data };
    }

    /// <summary>
    /// Creates a successful response with no data
    /// </summary>
    /// <returns>ApiResponse instance</returns>
    public static ApiResponse<T> Success()
    {
        return new ApiResponse<T> { Succeeded = true };
    }

    /// <summary>
    /// Creates a failed response with an error message
    /// </summary>
    /// <param name="error">Error message</param>
    /// <returns>ApiResponse instance</returns>
    public static ApiResponse<T> Failure(string error)
    {
        return new ApiResponse<T> { Succeeded = false, Error = error };
    }
}