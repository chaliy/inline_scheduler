using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced
{
    public class When_schedule_some_daily_work
    {
        private Scheduler _scheduler;

        int foo1_calls;

        [TestFixtureSetUp]
        public void Given_scheduler_with_daily_item() {

            var context = new TestSchedulerContext();

            _scheduler = new Scheduler(context);

            var runsoon = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            var foo1 = JobFactory.Daily("Foo1", () => { Console.WriteLine("Foo1"); foo1_calls += 1; }, runsoon, description: "Description for Foo1");
            var foo2 = JobFactory.Daily("Foo2", () => Console.WriteLine("Foo2"), runsoon);

            _scheduler.Schedule(foo1);
            _scheduler.Schedule(foo2);

            _scheduler.Start();

            Wait.Until(() => foo1_calls > 0, 30);

            context.MoveToTommorrow();

            Wait.Until(() => foo1_calls > 1, 30);

            context.MoveForwardNDays(2);

            Wait.Until(() => foo1_calls > 2, 50);
        }

        [Test]
        public void Should_run_once_each_day() {
            foo1_calls.Should().Be(3);
        }
    }
}
