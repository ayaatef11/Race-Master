using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RunGroop.Infrastructure.Workers;

public abstract class BackgroundWorker(
    ILogger<BackgroundWorker> _logger,
    Func<CancellationToken, Task> _perform) : BackgroundService
{ 

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Run(async () =>
        {
            await Task.Yield();
            _logger.LogInformation("Background worker started...");
            await _perform(stoppingToken).ConfigureAwait(false);
            _logger.LogInformation("Background worker stopped...");
        }, stoppingToken);
}