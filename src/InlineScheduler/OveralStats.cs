using System;

namespace InlineScheduler
{    
    public class OveralStats
    {
        public bool IsStopped { get; set; }
        public int PendingJobs { get; set; }
        public int ScheduledJobs { get; set; }
        public int RunningJobs { get; set; }
        public int JobsCount { get; set; }
        public DateTime StartTime { get; set; }
        
        public override string ToString()
        {
            return "Pending: " + PendingJobs + "; Scheduled: " + ScheduledJobs + " Running: " + RunningJobs + "; Current: " + JobsCount;
        }
    }
}
