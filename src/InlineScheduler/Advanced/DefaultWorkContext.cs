using System;
using InlineScheduler.Advanced.State;

namespace InlineScheduler.Advanced
{
    public class DefaultWorkContext : IWorkContext
    {
        private readonly Func<DateTime> _currentTime = (() => DateTime.Now);
        private readonly Random _rand = new Random();
        private readonly IStateProvider _state = new MemoryStateProvider();

        public DefaultWorkContext(){ }        

        public DateTime CurrentTime { get { return _currentTime(); } }

        public State.IStateProvider State { get { return _state; } }

        public int GetNextRandom(int from, int to)
        {
            return _rand.Next(from, to);
        }
        
    }
}
