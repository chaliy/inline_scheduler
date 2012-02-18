using System;

namespace InlineScheduler.Advanced
{
    public interface IJobState
    {
        JobStatus Status { get; }
        DateTime Created { get; }
        DateTime? LastStart { get; }
        DateTime? LastComplete { get; }
    }
}