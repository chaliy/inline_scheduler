using System;

namespace InlineScheduler.Advanced.State
{
    public class WorkState
    {
        public WorkState() { } //noop
        
        public WorkState(DateTime lastComplete) 
        {
            LastCompleteTime = lastComplete;
        }

        public DateTime? LastCompleteTime { get; set; }
    }
}
