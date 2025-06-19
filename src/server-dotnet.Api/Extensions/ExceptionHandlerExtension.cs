using server_dotnet.Middlewares;
using server_dotnet.Services.Interfaces;

namespace server_dotnet.Extensions;

public static class ExceptionHandlerExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            ExceptionHandler = new ExceptionHandlerMiddleware(
                    app.ApplicationServices.GetRequiredService<ILoggerFactory>(),
                    app.ApplicationServices.GetRequiredService<IActivityProvider>()
                        ).Invoke,
            AllowStatusCode404Response = true
        });

        return app;
    }
}
