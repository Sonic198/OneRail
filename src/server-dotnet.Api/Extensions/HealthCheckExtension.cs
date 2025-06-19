using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using server_dotnet.Constants;

namespace server_dotnet.Extensions;

public static class HealthCheckExtension
{
    public static IEndpointRouteBuilder MapHealthChecks(this IEndpointRouteBuilder endpoints)
    {        
        endpoints.MapHealthChecks(
            HealthCheck.Endpoints.AllDetails,
            new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        endpoints.MapHealthChecks(
            HealthCheck.Endpoints.LivenessProbe,
            new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(HealthCheck.Tags.Ready)
            });

        endpoints.MapHealthChecks(
            HealthCheck.Endpoints.ReadinessProbe,
            new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(HealthCheck.Tags.Services)
            });

        return endpoints;
    }

    public static IHealthChecksBuilder AddHealthChecks(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        string buildVersion)
    {
        return services.AddBaseHealthCheck(
            environment.ApplicationName,
            environment.EnvironmentName,
            buildVersion);
    }

    private static IHealthChecksBuilder AddBaseHealthCheck(
        this IServiceCollection services,
        string applicationName,
        string environmentName,
        string buildVersion)
    {
        return services.AddHealthChecks()
            .AddCheck(
                applicationName,
                () => HealthCheckResult.Healthy(),
                [
                    HealthCheck.Tags.Ready,
                    environmentName,
                    $"version {buildVersion}",
                    $".Net {Environment.Version}"
                ]);
    }
}

