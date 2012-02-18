using System;

namespace InlineScheduler.Advanced
{
    public interface IJobSchedule
    {
        DateTime? NextExecution(IJobState state, ISchedulerContext context);
    }
}