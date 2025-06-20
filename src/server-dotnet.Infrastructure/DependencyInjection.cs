using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Infrastructure.Data;
using server_dotnet.Infrastructure.Data.Interceptors;
using server_dotnet.Infrastructure.Services;

namespace server_dotnet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration,
       IHostEnvironment env)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        services.AddScoped<ISaveChangesInterceptor, LogChangesInterceptor>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
