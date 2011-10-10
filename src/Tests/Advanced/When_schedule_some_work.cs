using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace InlineScheduler.Tests.Advanced
{
    public class When_schedule_some_work
    {
        private Scheduler _scheduler;

        [TestFixtureSetUp]
        public void Given_scheduler_with_some_work()
        {
            _scheduler = new Scheduler();

            _scheduler.Schedule("Foo1", () => Console.WriteLine("Foo1"), TimeSpan.FromMinutes(2), description: "Description for Foo1");
            _scheduler.Schedule("Foo2", () => Console.WriteLine("Foo2"), TimeSpan.FromMinutes(2));
        }
        
        [Test]
        public void Should_schedule_all_work_items()
        {
            _scheduler.Stats.CurrentJobs.Should().HaveCount(2);
        }

        [Test]
        public void Should_add_description_to_work_item()
        {
            _scheduler.Stats.CurrentJobs.First(x => x.WorkKey == "Foo1").Description.Should().Be("Description for Foo1");
        }
    }
}
