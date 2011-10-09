using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace InlineScheduler.Advanced
{
    public class WorkBag : IEnumerable<WorkItem>
    {
        private readonly ConcurrentBag<WorkItem> _items = new ConcurrentBag<WorkItem>();

        public IEnumerator<WorkItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IList<WorkItem> GetApplicableToRun(int page)
        {
            return _items
                    .Reverse() // WTF? This is temporary, until we do not have priority stuff
                    .Where(x => x.Status == WorkStatus.Scheduled)
                    .Take(page)
                    .ToList();
        }

        public void UpdateState() 
        {
            foreach(var item in _items)
            {
                item.UpdateState();                
            }
        }

        public int GetRuningWork()
        {
            return _items.Count(x => x.Status == WorkStatus.Running);                    
        }

        public void Add(string workKey, Func<Task> factory, TimeSpan interval)
        {
            if (!_items.Any(x => x.WorkKey == workKey))
            {                
                _items.Add(new WorkItem(new WorkContext(), workKey, factory)
                              {
                                  Interval = interval
                              });
            }
        }        
    }
}
