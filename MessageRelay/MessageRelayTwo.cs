using MassTransit;
using MessageRelay.messages;

namespace MessageRelay;

public class MessageRelayTwo : IConsumer<MessageRelayOneMessage>
{
    private readonly ILogger<MessageRelayTwo> _logger;
    private readonly IBus _bus;

    public MessageRelayTwo(ILogger<MessageRelayTwo> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<MessageRelayOneMessage> context)
    {
        _logger.LogInformation("Message received: {0}, {1}", context.MessageId, context.Message);

        await Task.Delay(2000);
        
        _logger.LogInformation("Sending MessageRelayTwoMessage...");
        
        await _bus.Publish(new MessageRelayTwoMessage());
        
        _logger.LogInformation("Sent MessageRelayTwoMessage");
    }
}