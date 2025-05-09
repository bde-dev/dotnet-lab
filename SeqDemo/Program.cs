using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, logging) =>
{
    logging.ReadFrom.Configuration(context.Configuration);
    logging.WriteTo.OpenTelemetry(opts =>
    {
        opts.Endpoint = "http://localhost:5341/ingest/otlp/v1/logs";
        opts.Protocol = OtlpProtocol.HttpProtobuf;
        opts.Headers = new Dictionary<string, string>()
        {
            ["X-Seq-ApiKey"] = "CfqxpB1rlI8CREwlUoNu"
        };
    });
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
    {
        // TODO: Figure out how to get the neat little annotations added to the log entries like the above example: MICROSOFT PROVIDER SEQ
        resource.AddService("seq-demo");
        resource.AddTelemetrySdk();
    })
    .WithTracing(tracing =>
    {
        tracing.AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation();
        tracing.AddSource("seq-demo");
        tracing.AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/traces");
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
            options.Headers = "X-Seq-ApiKey=CfqxpB1rlI8CREwlUoNu";
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (ILogger<Program> logger) =>
    {
        logger.LogInformation("Getting weather forecast");
        
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        logger.LogInformation("Got forecast: {@forecast}", forecast);
        
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.UseSerilogRequestLogging();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}