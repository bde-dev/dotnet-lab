using System.Diagnostics.Metrics;

namespace OpenTelemetryDemo;

public static class DiagnosticsConfig
{
    public const string ServiceName = "Weather";

    public static Meter Meter = new(ServiceName);

    public static Counter<int> Counter = Meter.CreateCounter<int>("forecasts.count");
}