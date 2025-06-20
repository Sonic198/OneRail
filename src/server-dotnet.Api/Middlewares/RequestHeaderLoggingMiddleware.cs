namespace server_dotnet.Middlewares;

public class RequestHeaderLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestHeaderLoggingMiddleware> _logger;

    public RequestHeaderLoggingMiddleware(
        RequestDelegate next, 
        ILogger<RequestHeaderLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogDebug("Request {Method} {Path} | Headers: {Headers}",
            context.Request.Method,
            context.Request.Path,
            context.Request.Headers);

        await _next(context);
    }
}
