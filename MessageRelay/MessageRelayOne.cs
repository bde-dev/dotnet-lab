using MassTransit;
using MessageRelay.messages;

namespace MessageRelay;

public class MessageRelayOne : BackgroundService
{
    private readonly ILogger<MessageRelayOne> _logger;
    private readonly IBus _bus;

    public MessageRelayOne(ILogger<MessageRelayOne> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Sending MessageRelayOneMessage...");
            
            await _bus.Publish(new MessageRelayOneMessage(), cancellationToken: stoppingToken);
            
            _logger.LogInformation("MessageRelayOneMessage sent");
            
            await Task.Delay(5000, stoppingToken);
        }
    }
}