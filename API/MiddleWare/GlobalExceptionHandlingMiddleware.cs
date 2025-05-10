using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                var correlationId = context.TraceIdentifier;
                var errorDetails = new ProblemDetails
                {
                    Status = (int)GetStatusCode(exception),
                    Title = GetErrorTitle(exception),
                    Detail = _env.IsDevelopment() ? exception.ToString() : "An unexpected error occurred.",
                    Instance = context.Request.Path,
                    Extensions = { ["correlationId"] = correlationId }
                };

                _logger.LogError(exception, "An unhandled exception occurred. CorrelationId: {CorrelationId}, Path: {Path}", correlationId, context.Request.Path);

                context.Response.StatusCode = errorDetails.Status ?? (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var responseBody = JsonSerializer.Serialize(errorDetails, _jsonOptions);
                await context.Response.WriteAsync(responseBody);
            }
            catch (Exception handlerException)
            {
                _logger.LogCritical(handlerException, "Exception occurred while handling another exception.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("A fatal error occurred.");
            }
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private static string GetErrorTitle(Exception exception)
        {
            return exception switch
            {
                ArgumentException => "Invalid Request",
                UnauthorizedAccessException => "Unauthorized",
                KeyNotFoundException => "Resource Not Found",
                _ => "Internal Server Error"
            };
        }
    }
}
