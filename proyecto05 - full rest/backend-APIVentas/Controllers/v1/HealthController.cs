using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace VentasAPI.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class HealthController(HealthCheckService healthCheckService) : ControllerBase
{
    /// <summary>Verifica el estado de salud de la API y sus dependencias</summary>
    /// <remarks>
    /// Retorna el estado de la API y la conexión a base de datos.
    /// Valores posibles: Healthy, Degraded, Unhealthy
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(HealthReportResponse), 200)]
    [ProducesResponseType(typeof(HealthReportResponse), 503)]
    public async Task<IActionResult> GetHealth()
    {
        var report = await healthCheckService.CheckHealthAsync();

        var response = new HealthReportResponse
        {
            Status = report.Status.ToString(),
            TotalDuration = report.TotalDuration.TotalMilliseconds,
            Results = report.Entries.Select(e => new HealthCheckEntry
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Description = e.Value.Description ?? string.Empty,
                Duration = e.Value.Duration.TotalMilliseconds
            }),
            Timestamp = DateTime.UtcNow
        };

        return report.Status == HealthStatus.Healthy
            ? Ok(response)
            : StatusCode(503, response);
    }
}

public record HealthReportResponse
{
    public string Status { get; init; } = string.Empty;
    public double TotalDuration { get; init; }
    public IEnumerable<HealthCheckEntry> Results { get; init; } = [];
    public DateTime Timestamp { get; init; }
}

public record HealthCheckEntry
{
    public string Name { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public double Duration { get; init; }
}
