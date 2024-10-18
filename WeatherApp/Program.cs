using System.Reflection;
using LokiDemo;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using WeatherApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddMetrics();

builder.Host.UseSerilog((context, logging) =>
{
    logging
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
                { "service.name", "WeatherApp" },
                { "node.type", "table" },
            };
        })
        ;
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
    {
        resource.AddService("WeatherApp");
        resource.AddTelemetrySdk();
    })
    
    .WithTracing(tracing =>
    {
        tracing
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

            .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNet.Server.Kestrel", "WeatherApp")

            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri("http://192.168.252.66:4317");
                opts.Protocol = OtlpExportProtocol.Grpc;
            });
    })
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();