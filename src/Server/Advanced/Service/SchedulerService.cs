using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net.Http;
using InlineScheduler.Server.Advanced.Service.Content;

namespace InlineScheduler.Server.Advanced.Service
{
    [ServiceContract]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]  
    public class SchedulerService
    {

        private readonly Scheduler _scheduler;

        public SchedulerService(Scheduler scheduler)
        {
            _scheduler = scheduler;
        }

        // Static stuff
        [WebGet(UriTemplate = "/{*path}")]
        public HttpResponseMessage Get(string path = "index.html")
        {
            return Accessor.Get(path);
        }

        //[WebGet(UriTemplate = "Stats/")]
        //public SchedulerStats Stats()
        //{
        //    return _scheduler.GatherStats("all");
        //}

        [WebGet(UriTemplate = "Stats/List/{filter}/")]
        public SchedulerStats FilteredStats(string filter)
        {
            return _scheduler.GatherStats((filter ?? "all").Trim().ToLower());
        }

        [WebGet(UriTemplate = "Stats/Job/{workKey}/")]
        public SchedulerJobStats WorkStats(string workKey)
        {
            return _scheduler.GatherJobStats(workKey);
        }

        [WebInvoke(Method = "POST", UriTemplate = "Work/{workKey}/Force")]
        public void Force(string workKey)
        {
            _scheduler.Force(workKey);
        }

        [WebInvoke(Method = "POST", UriTemplate = "Stop")]
        public void Stop()
        {
            _scheduler.Stop();
        }

        [WebInvoke(Method = "POST", UriTemplate = "Start")]
        public void Start()
        {
            _scheduler.Start();
        }
    }
}
