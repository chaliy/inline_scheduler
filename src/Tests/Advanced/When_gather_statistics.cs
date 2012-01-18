using System;
using FluentAssertions;
using NUnit.Framework;

namespace InlineScheduler.Tests.Advanced
{
    public class When_gather_statistics
    {
        private Scheduler _scheduler;

        [TestFixtureSetUp]
        public void Given_schduler_with_some_work()
        {
            _scheduler = new Scheduler();

            _scheduler.Schedule("Foo1", () => Console.WriteLine("Foo1"), TimeSpan.FromMinutes(2));
            _scheduler.Schedule("Foo2", () => Console.WriteLine("Foo2"), TimeSpan.FromMinutes(2));
        }

        [Test]
        public void Should_generate_stats()
        {
            _scheduler.GatherStats().Should().NotBeNull();
        }

        [Test]
        public void Should_stats_for_all_work_items()
        {
            _scheduler.GatherStats().CurrentJobs.Should().HaveCount(2);
        }        
    }
}
