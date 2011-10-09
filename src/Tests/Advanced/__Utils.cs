using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System;
namespace InlineScheduler.Tests.Advanced
{
    class __Utils
    {        
    }

    class WorkItemFactory
    {

        public WorkItemFactory()
        {
            CurrentTime = DateTime.Now;
        }

        public DateTime CurrentTime { get; set; }        

        public WorkItem Create() 
        {
            return new WorkItem(new WorkContext(() => CurrentTime), "Foo1", () =>
            {
                return Task.Factory.StartNew(() => Console.WriteLine("Foo1"));
            });
        }
    }
}
