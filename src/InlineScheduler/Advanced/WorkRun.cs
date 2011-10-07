using System;

namespace InlineScheduler.Advanced
{
    public class WorkRun
    {
        public DateTime Started { get; set; }
        public DateTime Completed { get; set; }

        public WorkRunResult Result { get; set; }
        public string ResultMessage { get; set; }
    }
}
