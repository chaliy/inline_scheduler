using System;
using System.Collections.Generic;

namespace InlineScheduler
{
    public class SchedulerJobStats
    {
        public string JobKey { get; set; }
        public string Description { get; set; }
        public JobStatus CurrentStatus { get; set; }

        public DateTime? LastRunStarted { get; set; }
        public DateTime? LastRunCompleted { get; set; }

        public List<SchedulerJobRunStats> PreviousRuns { get; set; }

        public override string ToString() {
            return "JobKey: " + JobKey + " Status: " + CurrentStatus + " LastRunStarted: " + LastRunStarted + " LastRunCompleted: " + LastRunCompleted + " PreviousRuns Count: " + PreviousRuns.Count;
        } 
    }    
}
