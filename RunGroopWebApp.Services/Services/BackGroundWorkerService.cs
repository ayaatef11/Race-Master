

namespace RunGroopWebApp.Services.Services
{
    public class BackGroundWorkerService(ILogger<BackGroundWorkerService> _logger) : IHostedService
    {
      
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started ");
            Task.Delay(1000,cancellationToken).Wait();
            return Task.CompletedTask;  
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Stopped");
            return Task.CompletedTask;  
        }
    }
}
