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

            if (state.LastComplete != null) 
            {
                return state.LastComplete.Value.Date.AddDays(1).Add(_dayTime);
            }

            return context.GetCurrentTime().Date.Add(_dayTime);
        }
    }
}