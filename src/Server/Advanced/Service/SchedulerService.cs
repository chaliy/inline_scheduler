using System;
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

        [WebGet(UriTemplate = "Stats")]
        public SchedulerStats PostScheduled()
        {
            return _scheduler.Stats;
        }

        [WebInvoke(Method = "POST", UriTemplate = "Work/{workKey}/Force")]
        public void Force(string workKey)
        {
            _scheduler.Force(workKey);
        }
    }
}
