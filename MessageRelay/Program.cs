using System.Reflection;
using MassTransit;
using MessageRelay;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .MinimumLevel.Information()

        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", hostingContext.HostingEnvironment.ApplicationName)
        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName)

        .WriteTo.Console()
        .WriteTo.OpenTelemetry(otelLogging =>
        {
            otelLogging.Endpoint = "http://localhost:4317";
            otelLogging.Protocol = OtlpProtocol.Grpc;
            otelLogging.ResourceAttributes = new Dictionary<string, object>
            {
                { "service.name", "MessageRelay" }
            };
        });
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
    {
        resource.AddTelemetrySdk();
        resource.AddService("MessageRelay");
        resource.AddTelemetrySdk();
        resource.AddEnvironmentVariableDetector();
        resource.AddAttributes(new Dictionary<string, object>
        {
            { "service.name", "MessageRelay" }
        });
    })
    .WithTracing(tracing =>
    {
        tracing.AddSource("MessageRelay");
        tracing.AddSource("MassTransit");

        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();

        tracing.AddOtlpExporter(exporter =>
        {
            exporter.Endpoint = new Uri("http://localhost:4317");
            exporter.Protocol = OtlpExportProtocol.Grpc;
        });
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation();

        metrics.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNet.Server.Kestrel", "WeatherApp");

        metrics.AddOtlpExporter(exporter =>
        {
            exporter.Endpoint = new Uri("http://localhost:4317");
            exporter.Protocol = OtlpExportProtocol.Grpc;
        });
    });

builder.Services.AddMassTransit(mt =>
{
    mt.SetKebabCaseEndpointNameFormatter();
    mt.AddConsumers(typeof(Program).Assembly);
    mt.UsingRabbitMq((context, config) =>
    {
        config.Host("localhost", cfg =>
        {
            cfg.Username("guest");
            cfg.Password("guest");
        });

        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddHostedService<MessageRelayOne>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
