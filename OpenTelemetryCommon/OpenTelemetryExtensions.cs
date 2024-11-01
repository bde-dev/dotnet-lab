using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace OpenTelemetryCommon;

public static class OpenTelemetryExtensions
{
    public static void AddOpenTelemetryInstrumentation(this WebApplicationBuilder builder,
        string? hostname = null, Dictionary<string, object>? resourceAttributes = null)
    {
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.OpenTelemetry(opts =>
            {
                opts.Endpoint = (hostname ?? "localhost") + 4318;
                opts.Protocol = OtlpProtocol.HttpProtobuf;
                if (resourceAttributes != null) opts.ResourceAttributes = resourceAttributes;
            });
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(Assembly.GetExecutingAssembly().GetName().Name!);
                resource.AddTelemetrySdk();
            })

            .WithTracing(tracing =>
            {
                tracing
                    .AddSource("MassTransit")
                    .AddSource("wolverine")
                    .AddSource(Assembly.GetExecutingAssembly().GetName().Name!)

                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()

                    .AddConsoleExporter()
                    .AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = new Uri((hostname ?? "localhost") + 4317);
                        opts.Protocol = OtlpExportProtocol.Grpc;
                    });
            })
            
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
    
                    .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNet.Server.Kestrel", "WeatherApp")
            
                    .AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = new Uri((hostname ?? "localhost") + 4317);
                        opts.Protocol = OtlpExportProtocol.Grpc;
                    });
            });
    }
}