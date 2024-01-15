using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace MiniCrm.Infrastructure.Middleware;

public sealed class ExceptionHandlingMiddleware 
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger , RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex is not null)
                _logger.LogUnknownError(ex);

            await HandleExceptionAsync(context, ex!);
        }
    }
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred.");

        //More log stuff        

        ExceptionResponse response = exception switch
        {
            ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
            KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
            UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(response,
            options: new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            }));
    }
}

public static partial class ExceptionHandlingMiddlewareLogger
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "An error occurred")]
    public static partial void LogUnknownError(this ILogger logger, Exception ex);
    [LoggerMessage(EventId = 2, Level = LogLevel.Debug, Message = "{Content}")]
    public static partial void LogApiException(this ILogger logger, Exception ex, string content);
}
