using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System;
using InlineScheduler.Advanced.State;

namespace InlineScheduler.Tests.Advanced
{
    class __Utils
    {        
    }

    class TestJobState : IJobState
    {
        public JobStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastStart { get; set; }
        public DateTime? LastComplete { get; set; }
    }

    class TestSchedulerContext : ISchedulerContext
    {
        DateTime _currentTime;
        public TestSchedulerContext()
        {
            _currentTime = DateTime.Now;
            State = new MemoryStateProvider();
        }
       
        public DateTime GetCurrentTime()
        {
            return _currentTime;
        }

        public IStateProvider State { get; set; }        

        public int GetNextRandom(int from, int to)
        {
            return new Random().Next(from, to);
        }

        // Helpers
        internal void MoveToTommorrow()
        {
            _currentTime = DateTime.Now.AddDays(1);
        }

        internal void MoveToYesterday()
        {
            _currentTime = DateTime.Now.AddDays(-1);
        }

        internal void MoveToNow()
        {
            _currentTime = DateTime.Now;
        }

        internal void SetTodayTime(TimeSpan timeSpan)
        {
            _currentTime = DateTime.Now.Date.Add(timeSpan);
        }

        internal void MoveForwardNDays(int i) 
        {
            _currentTime = DateTime.Now.AddDays(i);
        }
    }

    class WorkItemFactory
    {        
        public static JobItem Create(ISchedulerContext ctx)
        {
            var definition = JobFactory.Interval("Foo1", () =>
            {
                return Task.Factory.StartNew(() => Console.WriteLine("Foo1"));
            }, TimeSpan.FromMinutes(10), "Some");

            return new InlineScheduler.Advanced.JobItemFactory(ctx).Create(definition);
        }
    }
}
