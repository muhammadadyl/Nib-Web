namespace Nib.Career.RestApi.HealthChecks
{
    public class HealthCheckStatusData
    {
        public bool IsLive { get; set; } = true;
        public bool ApplicationStopping { get; set; } = false;
    }
}