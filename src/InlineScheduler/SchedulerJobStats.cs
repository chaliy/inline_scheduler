using System;
using System.Collections.Generic;

namespace InlineScheduler
{
    public class SchedulerJobStats
    {
        public string WorkKey { get; set; }
        public SchedulerJobStatus CurrentStatus { get; set; }

        public DateTime? LastRunStarted { get; set; }
        public DateTime? LastRunCompleted { get; set; }

        public List<SchedulerJobRunStats> PreviousRuns { get; set; }
    }    
}
