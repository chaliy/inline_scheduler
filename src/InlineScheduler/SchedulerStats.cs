using System.Collections.Generic;

namespace InlineScheduler
{
    public class SchedulerStats
    {
        public int PendingJobs { get; set; }
        public int RunningJobs { get; set; }

        public List<SchedulerJobStats> CurrentJobs { get; set; }

        public override string ToString()
        {
            return "Pending: " + PendingJobs + "; Running: " + RunningJobs + "; Current: " + CurrentJobs.Count;            
        }
    }
}
