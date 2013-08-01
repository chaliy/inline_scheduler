using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using InlineScheduler.Server.Advanced.Hosting;
using InlineScheduler.Server.Advanced.Service;
using WebApiContrib.Formatters.JsonNet;

namespace InlineScheduler.Server
{
    // Migration stuff
    [Obsolete("Use InlineSchedulerWebHost.Init() call")]
    public class InlineSchedulerServer { }

    public class InlineSchedulerWebHost
    {
        public static void Init(Scheduler scheduler, string prefix = "Scheduler/", bool useServerUI = true)
        {
            if (useServerUI)
            {
                // Add / to the prefix, otherwise UI will not work            
                prefix = prefix ?? "";
                if (!prefix.EndsWith("/"))
                {
                    prefix += "/";
                }

                if (prefix.StartsWith("/"))
                    prefix = prefix.Substring(1, prefix.Length - 1);

                var config = GlobalConfiguration.Configuration;
                SetUp(config);

                var nestedActivator = (IHttpControllerActivator)config.Services.GetService(typeof(IHttpControllerActivator));

                var controllerActivator = new SimpleControllerActicator(prefix, nestedActivator, scheduler);
                config.Services.Replace(typeof(IHttpControllerActivator), controllerActivator);

                // POSTS

                config.Routes.MapHttpRoute(
                    name: prefix + "InlineScheduler/Work/Force",
                    routeTemplate: prefix + "Work/{workKey}/Force",
                    defaults: new { x_inline_scheduler_prefx = prefix, controller = "Scheduler", action = "Force" }
                );

                config.Routes.MapHttpRoute(
                    name: prefix + "InlineScheduler/Stop",
                    routeTemplate: prefix + "Stop",
                    defaults: new { x_inline_scheduler_prefx = prefix, controller = "Scheduler", action = "Stop" }
                );

                config.Routes.MapHttpRoute(
                    name: prefix + "InlineScheduler/Start",
                    routeTemplate: prefix + "Start",
                    defaults: new { x_inline_scheduler_prefx = prefix, controller = "Scheduler", action = "Start" }
                );

                // GETS

                config.Routes.MapHttpRoute(
                    name: prefix + "InlineScheduler/Stats/Job",
                    routeTemplate: prefix + "Stats/Job/{workKey}",
                    defaults: new { x_inline_scheduler_prefx = prefix, controller = "Scheduler", action = "WorkStats" }
                );

                config.Routes.MapHttpRoute(
                    name: prefix + "InlineScheduler/Stats/List",
                    routeTemplate: prefix + "Stats/List/{filter}",
                    defaults: new { x_inline_scheduler_prefx = prefix, controller = "Scheduler", action = "FilteredStats" }
                );

                // DEFAULT
                config.Routes.MapHttpRoute(
                    name: prefix + "InlineScheduler/Default",
                    routeTemplate: prefix + "{*path}",
                    defaults: new { x_inline_scheduler_prefx = prefix, controller = "Scheduler", action = "Default", path = RouteParameter.Optional }
                );
            }

            HostingRegistration.Register(scheduler);
        }

        static void SetUp(HttpConfiguration config)
        {
            // Making JSON a default format instead of XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Add(new JsonNetFormatter());

            var selfHosted = config as HttpSelfHostConfiguration;
            if (selfHosted != null)
            {
                selfHosted.MaxReceivedMessageSize = 16777216;
                selfHosted.MaxBufferSize = 16777216;
            }

            // For web hosted services set these parameters within web.config file
            //    <configuration>
            //        <system.web>
            //        <httpRuntime maxRequestLength="16384" requestLengthDiskThreshold="16384"/>
            //        </system.web>
            //    </configuration>
        }

        class SimpleControllerActicator : IHttpControllerActivator
        {
            readonly string _prefix;
            readonly IHttpControllerActivator _nestedActivator;
            readonly Scheduler _scheduler;

            public SimpleControllerActicator(string prefix, IHttpControllerActivator nestedActivator, Scheduler scheduler)
            {
                _prefix = prefix;
                _nestedActivator = nestedActivator;
                _scheduler = scheduler;
            }

            public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
            {
                if (controllerType == typeof(SchedulerController) && PrefixMatches(request))
                    return new SchedulerController(_scheduler);

                if (controllerType == typeof(SchedulerController) && _nestedActivator.GetType() != typeof(SimpleControllerActicator))
                {
                    // We can't activate CommandsController
                    throw new InvalidOperationException("Couldn't activate SchedulerController instance");
                }

                return _nestedActivator.Create(request, controllerDescriptor, controllerType);
            }

            bool PrefixMatches(HttpRequestMessage request)
            {
                if (!request.Properties.ContainsKey("MS_HttpRouteData"))
                    throw new InvalidOperationException("Couldn't get MS_HttpRouteData property");

                var route = request.Properties["MS_HttpRouteData"] as IHttpRouteData;

                if (route == null)
                    throw new InvalidOperationException("Unrecognized MS_HttpRouteData property type");

                if (!route.Values.ContainsKey("x_inline_scheduler_prefx"))
                    throw new InvalidOperationException("Missing x_inline_scheduler_prefx property");

                var prefix = (string)route.Values["x_inline_scheduler_prefx"];

                return prefix == _prefix;
            }
        }
    }
}
