using GrafanaStackSandbox;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<LogEmitter>();
builder.Services.AddHttpClient<WeatherService>();

builder.Services.AddMetrics();


//-----------------------------\\
// Common variable definitions \\
//-----------------------------\\

string otelCollectorHttpEndpoint = "http://192.168.252.66:4318";
string otelCollectorGrpcEndpoint = "http://192.168.252.66:4317";
var otelCollectorHttpEndpointUri = new Uri(otelCollectorHttpEndpoint);
var otelCollectorGrpcEndpointUri = new Uri(otelCollectorGrpcEndpoint);




//----------------------------\\
// OTLP logging configuration \\
//----------------------------\\

// Use the Serilog Open Telemetry sink to handle exporting logs via OTLP
builder.Host.UseSerilog((context, logging) =>
{
    logging
        .MinimumLevel.Verbose()
        .WriteTo.Console()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        
        
        
        //------------------------------------\\
        // Serilog OTLP logging configuration \\
        //------------------------------------\\
        
         // WRITE DIRECTLY TO LOKI   
        //.WriteTo.GrafanaLoki("http://192.168.252.66:3100", [new LokiLabel { Key = "app", Value = "sandbox" }])
        
        // WRITE TO AN OPEN TELEMETRY ENDPOINT SUCH AS THE OTEL COLLECTOR   
        .WriteTo.OpenTelemetry(opts =>
        {
            opts.Endpoint = otelCollectorHttpEndpoint;
            opts.Protocol = OtlpProtocol.HttpProtobuf;
            opts.ResourceAttributes = new Dictionary<string, object>()
            {
                ["service.name"] = "sandbox",
                ["node.type"] = "table"
            };
        })
        ;
});




//-----------------------------\\
// OTLP .NET SDK configuration \\
//-----------------------------\\

//The below configuration uses the .NET open telemetry SDK.

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
    {
        resource.AddService("sandbox");
        resource.AddAttributes(new Dictionary<string, object>() { ["node_type"] = "table" });
        resource.AddTelemetrySdk();
        resource.AddProcessDetector();
        resource.AddContainerDetector();
        resource.AddHostDetector();
        resource.AddOperatingSystemDetector();
        resource.AddEnvironmentVariableDetector();
    })
    
    
    //----------------------------\\
    // OTLP tracing configuration \\
    //----------------------------\\
    
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
    
        tracing.AddOtlpExporter(options =>
        {
            options.Endpoint = otelCollectorGrpcEndpointUri;
            options.Protocol = OtlpExportProtocol.Grpc;
        });
    })
    
    
    //----------------------------\\
    // OTLP metrics configuration \\
    //----------------------------\\
    
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
    
            .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNet.Server.Kestrel", "WeatherApp")
            
            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = otelCollectorGrpcEndpointUri;
                opts.Protocol = OtlpExportProtocol.Grpc;
            });
    })
    ;


var app = builder.Build();

//app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", async (ILogger<Program> logger, WeatherService weatherService) =>
    {
        logger.LogInformation("Calling Weather service");

        var forecast = await weatherService.GetWeatherForecast();
        
        logger.LogInformation("Got weather forecast {@forecast}", forecast);
        
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();
