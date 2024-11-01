using System.Diagnostics.Metrics;
using MassTransit;
using OpenTelemetryCommon;

namespace WeatherAlertReporting;

public class WeatherForecastConsumer(ILogger<WeatherForecastConsumer> logger, IMeterFactory meterFactory) : IConsumer<WeatherForecast>
{
    public async Task Consume(ConsumeContext<WeatherForecast> context)
    {
        await Task.Delay(100);
        
        var lAlertCounterMeter = meterFactory.Create("WeatherAlerts");
        var lInstrument = lAlertCounterMeter.CreateCounter<int>("weather_alerts_counter");
        lInstrument.Add(1);
        
        logger.LogInformation("WeatherAlert consumed: {@ForecastMessageReceived}", context.Message);
    }
}