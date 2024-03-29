using Serilog.Core;
using Serilog.Events;

namespace GhostNetwork.Profiles.Api.Helpers;

public class UtcTimestampEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
    {
        logEvent.AddPropertyIfAbsent(pf.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
    }
}
