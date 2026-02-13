using System.Diagnostics;

namespace Rindo.API.Middleware.Logging;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next(context);
            stopwatch.Stop();
            
            logger.LogInformation("Request: {@method} {@path}; EXECUTION TIME: {@elapsedTime}", context.Request.Method, context.Request.Path, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError("Exception: {@exception}", ex);
        }
    }
}