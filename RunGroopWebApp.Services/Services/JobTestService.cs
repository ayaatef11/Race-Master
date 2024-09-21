using Microsoft.Extensions.Logging;
using RunGroopWebApp.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroopWebApp.Services.Services
{
    public class JobTestService(ILogger<JobTestService> _logger) : IjobTestService
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
