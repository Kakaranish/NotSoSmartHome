using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using NotSoSmartHome.API.Configuration;

namespace NotSoSmartHome.API.HealthChecks;

public class PumpAgentHealthCheck : IHealthCheck
{
    private readonly IOptionsMonitor<PumpAgentOptions> _pumpAgentOptionsMonitor;
    private readonly IHttpClientFactory _httpClientFactory;

    public PumpAgentHealthCheck(
        IOptionsMonitor<PumpAgentOptions> pumpAgentOptionsMonitor,
        IHttpClientFactory httpClientFactory)
    {
        _pumpAgentOptionsMonitor = pumpAgentOptionsMonitor;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = new())
    {
        var agentUri = _pumpAgentOptionsMonitor.CurrentValue.Uri;
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(agentUri);
        var response = await httpClient.GetAsync("/health", cancellationToken);

        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }
}