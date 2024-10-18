using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Formatter;
using Serilog;
using Wolverine;
using Wolverine.MQTT;
using WolverineMqttDemo.Publishers;

namespace WolverineMqttDemo;

public record CreateCustomerMessage(Guid Id, string FullName);

internal static class Program
{
    public static void Main(string[] args)
    {
        var lBuilder = Host.CreateDefaultBuilder();
        

        lBuilder.UseWolverine(opts =>
        {
            opts.UseMqtt(mqtt =>
            {
                mqtt.WithClientOptions(client =>
                {
                    client.WithTcpServer("localhost");
                    client.WithClientId("dotnet-app");
                    client.WithCredentials("artemis", "artemis");
                });
            });

            opts.ListenToMqttTopic("testtopic");
        });

        lBuilder.ConfigureServices(s =>
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();

            s.AddSingleton(Log.Logger);

            s.AddHostedService<BackgroundPublisher>();
        });

        var lApp = lBuilder.Build();

        lApp.Run();
    }
}