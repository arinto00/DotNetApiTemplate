using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace ApiTemplate.API.Extensions;

/// <summary>
/// Health check for the API itself
/// </summary>
public class SelfHealthCheck : IHealthCheck
{
    /// <summary>
    /// Performs the health check
    /// </summary>
    /// <param name="context">Health check context</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Health check result</returns>
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        // You can add more meaningful health checks here
        return Task.FromResult(HealthCheckResult.Healthy("API is running"));
    }
}