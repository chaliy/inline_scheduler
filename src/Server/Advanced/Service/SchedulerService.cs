using System;
using System.ServiceModel;
using System.ServiceModel.Web;

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

        [WebInvoke(Method = "GET", UriTemplate = "Stats")]
        public SchedulerStats PostScheduled()
        {
            return _scheduler.Stats;
        }
    }
}
