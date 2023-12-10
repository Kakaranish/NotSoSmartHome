using Microsoft.Extensions.Diagnostics.HealthChecks;
using RaspberryAgent.Gpio;

namespace RaspberryAgent;

public class GpioHealthCheck : IHealthCheck
{
    private readonly GpioControllerAccessor _gpioControllerAccessor;

    public GpioHealthCheck(GpioControllerAccessor gpioControllerAccessor)
    {
        _gpioControllerAccessor = gpioControllerAccessor;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = new())
    {
        var isHealthy = _gpioControllerAccessor.IsGpioAccessible();
        
        return Task.FromResult(isHealthy
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy());
    }
}