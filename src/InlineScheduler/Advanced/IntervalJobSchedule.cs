using System;

namespace InlineScheduler.Advanced
{
    public class IntervalJobSchedule : IJobSchedule
    {
        readonly TimeSpan _interval;

        public IntervalJobSchedule(TimeSpan interval)
        {
            _interval = interval;
        }

        public DateTime? NextExecution(IJobState state, ISchedulerContext context)
        {
            if (state.Status != JobStatus.Pending)
            {
                return null; // Have no clue
            }

            if (state.LastComplete != null)
            {
                return state.LastComplete + _interval;                
            }
            else
            {
                var shift = context.GetNextRandom(200, (int) _interval.TotalMilliseconds/3);
                return state.Created.AddMilliseconds(shift);
            }
        }
    }
}