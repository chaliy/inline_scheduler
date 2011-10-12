using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System;

namespace InlineScheduler.Tests.Advanced
{
    class __Utils
    {        
    }

    class TestWorkContext : IWorkContext
    {
        public DateTime CurrentTime { get; set; }

        public TestWorkContext()
        {
            CurrentTime = DateTime.Now;
        }

        public void MoveToTommorrow()
        {
            CurrentTime = DateTime.Now.AddDays(1);
        }
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
            return new WorkItem(new DefaultWorkContext(() => CurrentTime), "Foo1", () =>
            {
                return Task.Factory.StartNew(() => Console.WriteLine("Foo1"));
            }, TimeSpan.FromMinutes(10));
        }

        public void MoveToTommorrow()
        {
            CurrentTime = DateTime.Now.AddDays(1);
        }
    }
}
