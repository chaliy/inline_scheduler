using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System;
using InlineScheduler.Advanced.State;

namespace InlineScheduler.Tests.Advanced
{
    class __Utils
    {        
    }

    class TestWorkContext : IWorkContext
    {        
        public TestWorkContext()
        {
            CurrentTime = DateTime.Now;
            State = new MemoryStateProvider();
        }

        public DateTime CurrentTime { get; set; }

        public IStateProvider State { get; set; }        

        public int GetNextRandom(int from, int to)
        {
            return new Random().Next(from, to);
        }

        // Helpers
        internal void MoveToTommorrow()
        {
            CurrentTime = DateTime.Now.AddDays(1);
        }

        internal void MoveToYesterday()
        {
            CurrentTime = DateTime.Now.AddDays(-1);
        }

        internal void MoveToNow()
        {
            CurrentTime = DateTime.Now;
        }        
    }

    class WorkItemFactory
    {        
        public static WorkItem Create(IWorkContext ctx) 
        {
            return new InlineScheduler.Advanced.WorkItemFactory(ctx).Create("Foo1", () =>
            {
                return Task.Factory.StartNew(() => Console.WriteLine("Foo1"));
            }, TimeSpan.FromMinutes(10), "Some");            
        }
    }
}
