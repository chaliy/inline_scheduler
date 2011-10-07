using System;

namespace InlineScheduler
{
    public class SchedulerJobRunStats
    {
        public DateTime Started { get; set; }
        public DateTime Completed { get; set; }

        public SchedulerJobRunResult Result { get; set; }
        public string ResultMessage { get; set; }
    }
}
