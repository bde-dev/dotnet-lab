using MassTransit;
using MessageRelay.messages;

namespace MessageRelay;

public class MessageRelayThree : IConsumer<MessageRelayTwoMessage>
{
    private readonly ILogger<MessageRelayThree> _logger;

    public MessageRelayThree(ILogger<MessageRelayThree> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MessageRelayTwoMessage> context)
    {
        _logger.LogInformation("Message received: {0}, {1}", context.MessageId, context.Message);
        await Task.Delay(100);
    }
}