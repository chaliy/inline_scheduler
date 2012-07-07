using System.Threading;
using FluentAssertions.Assertions;
using NUnit.Framework;
using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced
{
    public class When_do_some_work_items
    {
        private Scheduler _scheduler;

        private bool _foo1WorkDone;
        private bool _foo2WorkDone;

        [TestFixtureSetUp]
        public void Given_scheduler_with_some_work()
        {
            var ctx = new TestSchedulerContext();
            _scheduler = new Scheduler(ctx);

            _scheduler.Schedule("Foo1", () => { _foo1WorkDone = true; }, TimeSpan.FromMinutes(10));
            _scheduler.Schedule("Foo2", () => { _foo2WorkDone = true; }, TimeSpan.FromMinutes(10));                       
 
            // Lest ensure that all workitems 
            // are applicable for schedule
            ctx.MoveToTommorrow();

            _scheduler.Start();

            Wait.Until(() => _scheduler.GatherOveralStats().RunningJobs + _scheduler.GatherOveralStats().ScheduledJobs == 0);
        }

        [TestFixtureTearDown]
        public void Stop_scheduler()
        {
            _scheduler.Stop();
        }

        [Test]
        public void Should_do_work()
        {
            _foo1WorkDone.Should().BeTrue();
        }
    }
}
