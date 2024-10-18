using System.Diagnostics.Metrics;

namespace GrafanaStackSandbox;

public class WeatherService
{
    private readonly ILogger<WeatherService> _logger;
    private readonly HttpClient _httpClient;
    private readonly IMeterFactory _meterFactory;

    public WeatherService(ILogger<WeatherService> logger, HttpClient httpClient, IMeterFactory meterFactory)
    {
        _logger = logger;
        _httpClient = httpClient;
        _meterFactory = meterFactory;
    }

    public async Task<WeatherForecast> GetWeatherForecast()
    {
        var meter = _meterFactory.Create("WeatherApp");
        var instrument = meter.CreateCounter<int>("forecast_counter");
        instrument.Add(1);
        
        _logger.LogInformation("Calling openweathermap API");
        
        var apiKey = "04c17c216536cfd47706a94c615d8c58";

        var url = $"https://api.openweathermap.org/data/2.5/weather?q=london&appid={apiKey}&units=metric";
        
        _logger.LogInformation("Request URL: {@url}", url);

        var response = await _httpClient.GetAsync(url);
        
        var content = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode)
        {
            return new WeatherForecast(DateOnly.FromDateTime(DateTime.UtcNow), 5, "testing");
        }
        else
        {
            return new WeatherForecast(DateOnly.FromDateTime(DateTime.UtcNow), 5, "DEAD");
        }
    }
}