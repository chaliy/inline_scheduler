using System;
using InlineScheduler.Advanced;

namespace InlineScheduler.Tests.Advanced
{
    public class When_daily_schedule_
    {
        void Given_daily_schedule()
        {
            var schedule = new DailyJobSchedule(new TimeSpan(12, 0, 0));
            var context = new TestSchedulerContext();

            context.MoveToNow();
            //schedule.NextExecution(new TestJobState());

        }
    }
}
