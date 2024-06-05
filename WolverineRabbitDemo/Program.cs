using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Wolverine;
using Wolverine.RabbitMQ;

namespace WolverineRabbitDemo;

public record CreateCustomerMessage(Guid Id, string FullName);

class Program
{
    static void Main(string[] args)
    {
        var lBuilder = Host.CreateDefaultBuilder();

        lBuilder.UseWolverine(x =>
        {
            x.PublishMessage<CreateCustomerMessage>().ToRabbitExchange("customers-exc", exchange =>
            {
                exchange.ExchangeType = ExchangeType.Direct;
                exchange.BindQueue("customers-queue", "exchange2customers");
            });

            x.ListenToRabbitQueue("customers-queue").UseForReplies();

            x.UseRabbitMq(c =>
                {
                    c.HostName = "localhost";
                })
                .AutoProvision();
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

        var app = lBuilder.Build();
        
        app.Run();
    }
}