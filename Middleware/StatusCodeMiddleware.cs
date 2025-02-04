namespace CompClubAPI.Middleware;

public class StatusCodeMiddleware
{
    private readonly RequestDelegate _next;

    public StatusCodeMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        // Вызов следующего middleware
        await _next(context);
        // Логирование статус-кода
        Console.WriteLine($"Status Code: {context.Response.StatusCode}");
    }
}