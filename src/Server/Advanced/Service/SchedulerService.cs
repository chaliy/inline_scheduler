using System.Net.Http;
using System.Web.Http;
using InlineScheduler.Server.Advanced.Service.Content;

namespace InlineScheduler.Server.Advanced.Service
{
    public class SchedulerController : ApiController
    {
        readonly Scheduler _scheduler;

        public SchedulerController(Scheduler scheduler)
        {
            _scheduler = scheduler;
        }

        [HttpGet]
        public HttpResponseMessage Default(string path = "index.html")
        {
            return Accessor.Get(path);
        }

        [HttpGet]
        public SchedulerStats FilteredStats(string filter)
        {
            return _scheduler.GatherStats((filter ?? "all").Trim().ToLower());
        }

        [HttpGet]
        public SchedulerJobStats WorkStats(string workKey)
        {
            return _scheduler.GatherJobStats(workKey);
        }

        [HttpPost]
        public void Force(string workKey)
        {
            _scheduler.Force(workKey);
        }

        [HttpPost]
        public void Start()
        {
            _scheduler.Start();
        }

        [HttpPost]
        public void Stop()
        {
            _scheduler.Stop();
        }
    }
}
