using System.Web.Routing;
using InlineScheduler.Server.Advanced.Service;
using Microsoft.ApplicationServer.Http;
using WebApiContrib.Formatters.JsonNet;

namespace InlineScheduler.Server
{
    public class InlineSchedulerServer
    {
        public static void Init(Scheduler scheduler, string prefix = "Scheduler/")
        {
            // Add / to the prefix, otherwise UI will not work            
            prefix = prefix ?? "";
            if (!prefix.EndsWith("/"))
            {
                prefix += "/";
            }
           
        	var config = new HttpConfiguration();
            config.CreateInstance = (t, c, m) => new SchedulerService(scheduler);            

            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Remove(config.Formatters.JsonValueFormatter);            
            config.Formatters.Insert(0, new JsonNetFormatter());

            RouteTable.Routes.MapServiceRoute<SchedulerService>(prefix, config);
        }

    }
}
