namespace CompClubAPI.Middleware;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var route = context.Request.Path;
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var statusCode = context.Response.StatusCode;
        var colorCode = statusCode >= 200 && statusCode < 300 ? "\x1b[32m" : "\x1b[31m";
        _logger.LogInformation(
            $"Route: {route}, Status Code: {colorCode}{statusCode}\x1b[0m, Time Taken: {stopwatch.ElapsedMilliseconds} ms");
    }
}