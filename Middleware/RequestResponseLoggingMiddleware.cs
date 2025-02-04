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
        // Запоминаем маршрут
        var route = context.Request.Path;

        // Запоминаем время начала обработки запроса
        var stopwatch = Stopwatch.StartNew();

        // Вызываем следующий middleware в конвейере
        await _next(context);

        // Останавливаем таймер
        stopwatch.Stop();

        // Логируем статус-код и маршрут
        _logger.LogInformation($"Route: {route}, Status Code: {context.Response.StatusCode}, Time Taken: {stopwatch.ElapsedMilliseconds} ms");
    }
}