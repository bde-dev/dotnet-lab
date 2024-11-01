using System.Diagnostics.Metrics;
using MassTransit;
using OpenTelemetryCommon;

namespace WeatherApp;

public class WeatherService
{
    private readonly ILogger<WeatherService> _logger;
    private readonly HttpClient _httpClient;
    private readonly IBus _bus;
    private IMeterFactory _meterFactory;

    public WeatherService(ILogger<WeatherService> logger, HttpClient httpClient, IMeterFactory meterFactory, IBus bus)
    {
        _logger = logger;
        _httpClient = httpClient;
        _meterFactory = meterFactory;
        _bus = bus;
    }

    public async Task<WeatherForecast[]> GetWeatherForecast()
    {
        var meter = _meterFactory.Create("WeatherApp");
        var instrument = meter.CreateCounter<int>("forecast_counter");
        instrument.Add(1);
        
        
        
        
        //#############################################
        // Simulate an external HTTP API request
        // Merely exists to add a span to the trace
        // The response data is discarded
        //#############################################
        
        _logger.LogInformation("Calling openweathermap API");
        
        var apiKey = "04c17c216536cfd47706a94c615d8c58";

        var url = $"https://api.openweathermap.org/data/2.5/weather?q=london&appid={apiKey}&units=metric";
        
        _logger.LogInformation("Request URL: {@url}", url);

        var response = await _httpClient.GetAsync(url);
        
        var content = await response.Content.ReadAsStringAsync();
        
        //#############################################
        // End arbitrary external HTTP API request
        //#############################################
        
        
        
        
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        var lForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray();
        
        _logger.LogInformation("Got forecasts: {@lForecasts}", lForecasts);

        await _bus.Publish(lForecasts);

        return lForecasts;
    }
}