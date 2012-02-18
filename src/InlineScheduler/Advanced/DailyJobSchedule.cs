using System;

namespace InlineScheduler.Advanced
{
    public class DailyJobSchedule : IJobSchedule
    {
        readonly TimeSpan _dayTime;

        public DailyJobSchedule(TimeSpan dayTime)
        {
            _dayTime = dayTime;
        }

        public DateTime? NextExecution(IJobState state, ISchedulerContext context)
        {
            if (state.Status != JobStatus.Pending)
            {
                return null; // Have no clue
            }

            return context.GetCurrentTime().Date.Add(_dayTime);
        }
    }
}