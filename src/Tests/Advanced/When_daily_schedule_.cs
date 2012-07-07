using System;
using InlineScheduler.Advanced;
using NUnit.Framework;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced
{
    public class When_daily_schedule_
    {
        DateTime? nexttoday;
        DateTime? nexttomorrow;

        [TestFixtureSetUp]
        public void Given_daily_schedule()
        {
            var schedule = new DailyJobSchedule(new TimeSpan(12, 0, 0));
            var context = new TestSchedulerContext();


            context.MoveToNow();

            var jobstate = new TestJobState();

            nexttoday = schedule.NextExecution(jobstate, context);

            context.MoveToTommorrow();

            nexttomorrow = schedule.NextExecution(jobstate, context);

        }

        [Test]
        public void next_time_should_be_midday() {
            nexttoday.HasValue.Should().BeTrue();
            nexttoday.Value.Hour.Should().Be(12);
            nexttoday.Value.Date.Day.Should().Be(DateTime.Now.Day);

            nexttomorrow.HasValue.Should().BeTrue();
            nexttomorrow.Value.Day.Should().Be(DateTime.Now.AddDays(1).Day);
        }
    }
}
