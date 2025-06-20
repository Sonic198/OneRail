using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace server_dotnet.Infrastructure.Data.Interceptors;

internal class LogChangesInterceptor : SaveChangesInterceptor
{
    private readonly ILogger<LogChangesInterceptor> _logger;
    private List<EntityEntry>? _capturedChanges;

    public LogChangesInterceptor(ILogger<LogChangesInterceptor> logger)
    {
        _logger = logger;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context != null)
        {
            _capturedChanges = context.ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (_capturedChanges != null)
        {
            foreach (var entry in _capturedChanges)
            {
                var entityName = entry.Entity.GetType().Name;
                Console.WriteLine($"Entity: {entityName}, State: {entry.State}");

                foreach (var prop in entry.Properties)
                {
                    var name = prop.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            _logger.LogInformation("Entity deleted: {EntityName}, Property: {PropertyName}, Original Value: {OriginalValue}", entityName, name, prop.OriginalValue);
                            break;
                        case EntityState.Modified:
                            _logger.LogInformation("Entity modified: {EntityName}, Property: {PropertyName}, Original Value: {OriginalValue}, Current Value: {CurrentValue}", entityName, name, prop.OriginalValue, prop.CurrentValue);
                            break;
                        case EntityState.Added:
                            _logger.LogInformation("Entity added: {EntityName}, Property: {PropertyName}, Current Value: {CurrentValue}", entityName, name, prop.CurrentValue);
                            break;
                        default:
                            break;
                    }
                }
            }

            _capturedChanges = null;
        }

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
