using Serilog;

namespace WolverineMqttDemo.Handlers;

public class CreateCustomerHandler
{
    private readonly ILogger _logger;

    public CreateCustomerHandler(ILogger logger)
    {
        _logger = logger;
    }

    public void Handle(List<CreateCustomerMessage> message)
    {
        _logger.Information("Handling create customer message: {0}", message.ToString());
    }
}