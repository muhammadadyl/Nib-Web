using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Nib.Career.RestApi
{
    public class UnhandledExceptionFilter : IAsyncExceptionFilter
    {
        private const string UnhandledError = "urn:nib:career:api:unhandled-error";

        private readonly ILogger<UnhandledExceptionFilter> _logger;

        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception");

            const int defaultStatusCode = StatusCodes.Status500InternalServerError;

            if (context.Exception is RpcException ex)
            {
                var statusCode = ex.StatusCode switch
                {
                    StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
                    StatusCode.NotFound => StatusCodes.Status404NotFound,
                    StatusCode.AlreadyExists => StatusCodes.Status409Conflict,
                    _ => defaultStatusCode
                };

                context.Result = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new ProblemDetails { Detail = ex.Status.Detail, Status = statusCode, Title = ex.ToString() }),
                    StatusCode = statusCode,
                    ContentType = "application/json+problem",
                };
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new ProblemDetails { Detail = UnhandledError, Status = defaultStatusCode }),
                    StatusCode = defaultStatusCode,
                    ContentType = "application/json+problem"
                };
            }

            return Task.CompletedTask;
        }
    }
}