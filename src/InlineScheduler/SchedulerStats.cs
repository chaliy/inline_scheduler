using System.Collections.Generic;

namespace InlineScheduler
{
    public class SchedulerStats
    {
        public OveralStats Overal { get; set; }
        public List<MinimalJobStats> CurrentJobs { get; set; }

        public override string ToString()
        {
            if (Overal == null)
            {
                return "N/A";
            }
            return Overal.ToString();
        }

        public SchedulerStats()
        {
            Overal = new OveralStats();
            CurrentJobs = new List<MinimalJobStats>();
        }
    }    
}
