using System;
namespace InlineScheduler.Advanced
{
    public class DefaultWorkContext : InlineScheduler.Advanced.IWorkContext
    {
        private readonly Func<DateTime> _currentTime;

        public DefaultWorkContext() : this(null) { }

        public DefaultWorkContext(Func<DateTime> currentTime)
        {
            _currentTime = currentTime ?? (() => DateTime.Now);
        }

        public DateTime CurrentTime { get { return _currentTime(); } }        
    }
}
