using System;

namespace InlineScheduler.Advanced
{
    public class JobRun
    {
        public DateTime Started { get; set; }
        public DateTime Completed { get; set; }

        public JobRunResult Result { get; set; }
        public string ResultMessage { get; set; }
    }
}
