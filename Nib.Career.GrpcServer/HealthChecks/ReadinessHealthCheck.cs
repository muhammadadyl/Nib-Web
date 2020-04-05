using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Nib.Career.GrpcServer.HealthChecks
{
    public class ReadinessHealthCheck : IHealthCheck
    {
        private readonly HealthCheckStatusData _healthCheckStatusData;

        public ReadinessHealthCheck(HealthCheckStatusData healthCheckStatusData)
        {
            _healthCheckStatusData = healthCheckStatusData;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(_healthCheckStatusData.ApplicationStopping ? HealthCheckResult.Unhealthy("Application shutting down") : HealthCheckResult.Healthy());
        }
    }
}