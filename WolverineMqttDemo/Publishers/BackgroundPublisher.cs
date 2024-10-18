using Microsoft.Extensions.Hosting;
using Serilog;
using Wolverine;

namespace WolverineMqttDemo.Publishers;

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
            var lList = new List<CreateCustomerMessage>
            {
                new (Guid.NewGuid(), "Brad Evans")
            };
            
            await _messageBus.BroadcastToTopicAsync("testtopic", lList);
            _logger.Information("Published create customer message");
            await Task.Delay(2000, stoppingToken);
        }
    }
}