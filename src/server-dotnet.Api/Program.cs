using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Net.Http.Headers;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using server_dotnet.Application;
using server_dotnet.Constants;
using server_dotnet.Extensions;
using server_dotnet.Infrastructure;
using server_dotnet.Infrastructure.Data;
using server_dotnet.Middlewares;
using server_dotnet.Serilog;
using server_dotnet.Services;
using server_dotnet.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = SerilogHelper.CreateLogger(builder.Configuration);

builder.Host.UseSerilog();

try
{
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

    builder.Services.AddCors();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSingleton<IActivityProvider, ActivityProvider>();

    var healthCheckBuilder = builder.Services.AddHealthChecks(builder.Environment, "1.0.0")
        .AddSqlServer(
            builder.Configuration.GetConnectionString("SqlServer")!,
            tags: [HealthCheck.Tags.Services]);

    builder.Services.AddOpenApiDocument(config =>
    {
        config.AddSecurity(
                JwtConstants.TokenType,
                new OpenApiSecurityScheme
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = JwtConstants.TokenType,
                    Type = OpenApiSecuritySchemeType.Http,
                    Name = HeaderNames.Authorization,
                    Description = "Copy valid JWT token into field without 'Bearer' prefix",
                    In = OpenApiSecurityApiKeyLocation.Header
                });
        
        config.OperationProcessors.Add(new OperationSecurityScopeProcessor(JwtConstants.TokenType));
    });

    builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("organizationRateLimiter", opt =>
        {
            opt.PermitLimit = 30;
            opt.Window = TimeSpan.FromSeconds(60);
        });
    });
    //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-9.0&tabs=windows
    //dotnet user-jwts create
    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication("Bearer").AddJwtBearer();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        await using (var scope = app.Services.CreateAsyncScope())
        await using (var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
        {
            await dbContext.Database.EnsureCreatedAsync();
        }
    }

    app.UseCustomExceptionHandler();

    app.UseCors(policy 
        => policy.SetIsOriginAllowed(isOriginAllowed: _ => true)
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

    app.UseAuthorization();
    app.UseRateLimiter();

    app.UseMiddleware<RequestHeaderLoggingMiddleware>();

    app.RegisterEndpointDefinitions();

    app.MapHealthChecks();

    app.UseOpenApi();
    app.UseSwaggerUi();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Created for testing purposes
public partial class Program { }