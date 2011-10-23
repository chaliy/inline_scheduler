using System;

namespace InlineScheduler.Advanced
{
    public class DefaultWorkContext : IWorkContext
    {
        private readonly Func<DateTime> _currentTime;
        private readonly Random _rand = new Random();

        public DefaultWorkContext() : this(null) { }

        public DefaultWorkContext(Func<DateTime> currentTime)
        {
            _currentTime = currentTime ?? (() => DateTime.Now);
        }

        public DateTime CurrentTime { get { return _currentTime(); } }

        public int GetNextRandom(int from, int to)
        {
            return _rand.Next(from, to);
        }
    }
}
