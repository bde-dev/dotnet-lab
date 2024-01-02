using HelloWorldLibrary.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UltimateHelloWorldApp;

using IHost lHost = CreateHostBuilder(args).Build();
using var lScope = lHost.Services.CreateScope();

var lServices = lScope.ServiceProvider;

try
{
    lServices.GetRequiredService<App>().Run(args);
}
catch (Exception lException)
{
    Console.WriteLine(lException.Message);
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IMessages, Messages>();
            services.AddSingleton<App>();
        });
}