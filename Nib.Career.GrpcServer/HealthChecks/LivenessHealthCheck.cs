using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Nib.Career.GrpcServer.HealthChecks
{
    public class LivenessHealthCheck : IHealthCheck
    {
        private readonly HealthCheckStatusData _healthCheckStatusData;

        public LivenessHealthCheck(HealthCheckStatusData healthCheckStatusData)
        {
            _healthCheckStatusData = healthCheckStatusData;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(_healthCheckStatusData.IsLive ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy("Error"));
        }
    }
}