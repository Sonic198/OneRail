using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using server_dotnet.Extensions;
using server_dotnet.Responses;
using server_dotnet.Services.Interfaces;

namespace server_dotnet.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IActivityProvider _activityProvider;

    public ExceptionHandlerMiddleware(
        ILoggerFactory loggerFactory,
        IActivityProvider activityProvider)
    {
        _logger = loggerFactory?.CreateLogger<ExceptionHandlerMiddleware>()
            ?? throw new ArgumentNullException(nameof(loggerFactory));

        _activityProvider = activityProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (ex is null)
            return;

        var response = new ApiErrorResponse(ex.Message);

        response.TraceId = _activityProvider.Current.GetCurrentTraceIdentifier();

        context.Response.StatusCode = response.StatusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var bodyStream = context.Response.Body;
        await JsonSerializer.SerializeAsync(
            bodyStream,
            response,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        _logger.LogError(ex, "{Message}", ex.Message);
    }
}
