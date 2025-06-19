using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;
using server_dotnet.Extensions;

namespace server_dotnet.Serilog;

public class ActivityEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;
        if (activity is not null)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty("TraceIdentifier", new ScalarValue(activity.GetCurrentTraceIdentifier())));
        }
    }
}