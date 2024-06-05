using Serilog;

namespace WolverineMqttDemo.Handlers;

public class CreateCustomerHandler
{
    private readonly ILogger _logger;

    public CreateCustomerHandler(ILogger logger)
    {
        _logger = logger;
    }

    public void Handle(CreateCustomerMessage pCreateCustomerMessage)
    {
        _logger.Information(pCreateCustomerMessage.ToString());
    }
}