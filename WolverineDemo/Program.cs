// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Wolverine;
using Wolverine.MQTT;
using WolverineDemo.Publishers;

namespace WolverineDemo;

public record CreateCustomerMessage(Guid Id, string FullName);

internal static class Program
{
    public static void Main(string[] args)
    {
        var lBuilder = Host.CreateDefaultBuilder();

        lBuilder.UseWolverine(x =>
        {
            x.UseMqtt(c =>
            {
                c.WithClientOptions(c =>
                {
                    c.WithTcpServer("localhost");
                    c.WithClientId("dotnet-app");
                    c.WithCredentials("admin", "admin");
                });
            });

            x.PublishAllMessages().ToTopic("testtopic");
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