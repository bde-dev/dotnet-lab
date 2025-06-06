﻿using Serilog;

namespace WolverineRabbitDemo;

public class CreateCustomerHandler
{
    private readonly ILogger _logger;

    public CreateCustomerHandler(ILogger logger)
    {
        _logger = logger;
    }

    public void Handle(CreateCustomerMessage createCustomer)
    {
        _logger.Information(createCustomer.ToString());
    }
}