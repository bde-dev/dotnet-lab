using System.Reflection;
using MassTransit;
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

builder.Services.AddMetrics();

builder.Services.AddMassTransit(mt =>
{
    mt.SetKebabCaseEndpointNameFormatter();
    mt.AddConsumers(typeof(Program).Assembly);
    mt.UsingRabbitMq((context, config) =>
    {
        config.Host("localhost", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
        
        config.ConfigureEndpoints(context);
    });
});



builder.Host.UseSerilog((context, logging) =>
{
    logging
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .Enrich.WithProperty("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString())
        
        .WriteTo.Console()
        .WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = "http://192.168.252.66:4318";
            options.Protocol = OtlpProtocol.HttpProtobuf;
            options.ResourceAttributes = new Dictionary<string, object>
            {
                { "service.name", "WeatherAlerts" },
                { "node.type", "table" },
            };
        })
        ;
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
    {
        resource.AddService("WeatherAlerts");
        resource.AddTelemetrySdk();
    })
    
    .WithTracing(tracing =>
    {
        tracing
            .AddSource("MassTransit")
            
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation();

        tracing.AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://192.168.252.66:4317");
            options.Protocol = OtlpExportProtocol.Grpc;
        });
    })
    
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()

            .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNet.Server.Kestrel")

            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri("http://192.168.252.66:4317");
                opts.Protocol = OtlpExportProtocol.Grpc;
            });
    })
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();