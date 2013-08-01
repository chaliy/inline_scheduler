using System;
using System.Threading;
using System.Threading.Tasks;
using InlineScheduler;
using InlineScheduler.Server;

namespace SimpleWebExample
{
    public class Global : System.Web.HttpApplication
    {
        public readonly static Scheduler Instance = new Scheduler();

        protected void Application_Start(object sender, EventArgs e)
        {
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                var id = "Foo" + i;
                Instance.Schedule(id, () =>
                {
                    return Task.Factory.StartNew(() =>
                    {
                        if (id == "Foo3")
                        {
                            throw new InvalidOperationException("With love from Scheduler");
                        }
                        Console.WriteLine(id + " Started");
                        for (var a = 0; a < 10000 + random.Next(); a++)
                        {
                            Thread.Sleep(1);
                        }
                        Console.WriteLine(id + " Completed");
                    });
                }, TimeSpan.FromMinutes(3), description: "Do some stupid work with " + id);
            }

            InlineSchedulerWebHost.Init(Instance);
        }
    }
}