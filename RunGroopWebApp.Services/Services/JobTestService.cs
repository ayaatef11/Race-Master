
using RunGroopWebApp.Services.interfaces;

namespace RunGroopWebApp.Services.Services
{
    public class JobTestService(ILogger<JobTestService> _logger) 
    {

        public void ContinuationJob()
        {

            _logger.LogInformation("Hello from a Continuation job!");
        }
        public void DelayedJob()
        {

            _logger.LogInformation("Hello from a Delayed job!");
        }
        public void FireAndForgetJob()
        {

            _logger.LogInformation("Hello from a Fire and Forget job!");
        }
        public void ReccuringJob()
        {

            _logger.LogInformation("Hello from a Scheduled job!");
        }
    }
}
