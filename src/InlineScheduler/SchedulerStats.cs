using System.Collections.Generic;

namespace InlineScheduler
{
    public class SchedulerStats
    {
        public bool IsStopped { get; set; }
        public int PendingJobs { get; set; }
        public int ScheduledJobs { get; set; }
        public int RunningJobs { get; set; }

        public List<SchedulerJobStats> CurrentJobs { get; set; }

        public override string ToString()
        {
            return "Pending: " + PendingJobs + "; Scheduled: " + ScheduledJobs + " Running: " + RunningJobs + "; Current: " + CurrentJobs.Count;
        }
    }
}
