using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarRental.Automotor.Infrastructure.Middlewares
{
    public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

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

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception");

            var (statusCode, payload) = exception switch
            {
                ArgumentException => (
                    StatusCodes.Status400BadRequest, 
                    CreateProblemDetails(context, "Invalid Request")
                ),
                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    CreateProblemDetails(context, "Resource not found.")
                ),
                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    CreateProblemDetails(context, "Unauthorized.")
                ),
                BusinessException => (
                    StatusCodes.Status422UnprocessableEntity,
                    CreateProblemDetails(context, exception.Message)
                ),
                _ => (
                    StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(context, "An unexpected error occurred.")
                )
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(payload.ToString());
        }

        private static ProblemDetails CreateProblemDetails(HttpContext context, string title)
        {
            return new ProblemDetails
            {
                Title = title,
                Status = context.Response.StatusCode,
                Instance = context.Request.Path
            };
        }
    }
}
