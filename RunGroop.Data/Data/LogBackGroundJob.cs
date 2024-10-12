using Microsoft.Extensions.Logging;
using Quartz;

namespace RunGroop.Data.Data
{
    public class LogBackgroundJob : IJob
    {

        private readonly ILogger _logger;

        public LogBackgroundJob(ILogger<LogBackgroundJob> logger)  => _logger = logger;

        public Task Execute(IJobExecutionContext context)
        {

            _logger.LogInformation($"Logging job executed on {DateTime.UtcNow}");

            return Task.CompletedTask;

        }
    }
}
