using System.Collections.Generic;

namespace InlineScheduler.Advanced.State
{
    public class MemoryStateProvider : IStateProvider
    {
        private readonly IDictionary<string, WorkState> _store = new Dictionary<string, WorkState>();

        public void Store(string key, WorkState state)
        {
            _store[key] = state;
        }

        public WorkState Retrieve(string key)
        {
            if (_store.ContainsKey(key))
            {
                return _store[key];
            }
            return null;
        }
    }
}
