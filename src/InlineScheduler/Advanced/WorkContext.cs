using System;
namespace InlineScheduler.Advanced
{
    public class WorkContext
    {
        private readonly Func<DateTime> _currentTime;

        public WorkContext() : this(null) { }

        public WorkContext(Func<DateTime> currentTime)
        {
            _currentTime = currentTime ?? (() => DateTime.Now);
        }

        public DateTime CurrentTime { get { return _currentTime(); } }        
    }
}
