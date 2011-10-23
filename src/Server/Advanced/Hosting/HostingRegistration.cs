using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Threading;
using InlineScheduler.Server.Utility;

namespace InlineScheduler.Server.Advanced.Hosting
{    
    public class HostingRegistration : IRegisteredObject
    {
        // For more details http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx    

        private readonly Scheduler _underline;

        public HostingRegistration(Scheduler underline)
        {
            _underline = underline;
        }

        public void Stop(bool immediate)
        {
            if (immediate)
            {
                _underline.Stop();
                Do.TryWaitUntil(() => _underline.IsRunningJobsNow, TimeSpan.FromSeconds(15));
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public static void Register(Scheduler scheduler) 
        {
            HostingEnvironment.RegisterObject(new HostingRegistration(scheduler));
        }
    }
}
