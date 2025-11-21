using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BetterThanYou.Web.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var (statusCode, title) = exception switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, "Validation Error"),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Not found"),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
        };

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
        };
        
        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}