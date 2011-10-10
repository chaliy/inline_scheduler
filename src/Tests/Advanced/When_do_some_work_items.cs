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
            _scheduler = new Scheduler();

            _scheduler.Schedule("Foo1", () => { _foo1WorkDone = true; }, TimeSpan.FromMinutes(10));
            _scheduler.Schedule("Foo2", () => { _foo2WorkDone = true; }, TimeSpan.FromMinutes(10));

            _scheduler.Start();            

            Wait.For(1.Seconds());
            Wait.Unitl(() => _scheduler.Stats.RunningJobs + _scheduler.Stats.ScheduledJobs == 0);
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
