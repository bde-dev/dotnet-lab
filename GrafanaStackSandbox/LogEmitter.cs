namespace GrafanaStackSandbox;

public class LogEmitter(ILogger<LogEmitter> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Log emitter running.");
            await Task.Delay(1000, stoppingToken);
        }
    }
}