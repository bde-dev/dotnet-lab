using Microsoft.Extensions.Hosting;
using Serilog;
using Wolverine;

namespace WolverineRabbitDemo;

public class BackgroundPublisher : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IMessageBus _messageBus;

    public BackgroundPublisher(ILogger logger, IMessageBus messageBus)
    {
        _logger = logger;
        _messageBus = messageBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _messageBus.PublishAsync(new CreateCustomerMessage(new Guid(), "Brad Evans"));
            await Task.Delay(2000, stoppingToken);
        }
    }
}