using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Services.interfaces;
using RunGroopWebApp.Services.Services;

namespace RunGroopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTestController(JobTestService _jobTestService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager _recurringJobManager) : ControllerBase
    {

        [HttpGet("/FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {

            backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());

            return Ok();
        }
        [HttpGet("/DelayedJob")]
        public ActionResult CreateDelayedJob()
        {

            backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(60));

            return Ok();
        }

        [HttpGet("/ReccuringJob")]
        public ActionResult CreateReccuringJob()
        {

            _recurringJobManager.AddOrUpdate("jobId", () => _jobTestService.ReccuringJob(), Cron.Minutely);

            return Ok();
        }

        [HttpGet("/ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {

            var parentJobId = backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
            backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.ContinuationJob());

            return Ok();
        }
    }
}