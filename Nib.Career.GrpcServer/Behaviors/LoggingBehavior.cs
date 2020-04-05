using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Nib.Career.GrpcServer
{
    [DebuggerStepThrough]
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var start = Stopwatch.GetTimestamp();

             _logger.LogInformation("Handling request '{type}' with payload: {@payload}", typeof(TRequest).Name, request);
            
            try
            {
                var response = await next();
                LogCompletion(GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
                return response;
            }
            catch (Exception e) when (LogCompletion(GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), e))
            {
                throw;
            }
        }

        private bool LogCompletion(double duration, Exception exception = null)
        {
            if (exception != null) _logger.LogError(exception, "Failed to handle request '{type}' in {elapsedMs:0.0000}ms", typeof(TRequest).Name, duration);
            else _logger.LogInformation("Successfully handled request in {elapsedMs:0.0000}ms", duration);
            return false;
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}
