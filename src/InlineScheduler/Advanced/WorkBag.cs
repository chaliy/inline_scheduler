using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace InlineScheduler.Advanced
{
    public class WorkBag : IEnumerable<WorkItem>
    {
        private readonly IWorkContext _context;
        private readonly ConcurrentBag<WorkItem> _items = new ConcurrentBag<WorkItem>();

        public WorkBag(IWorkContext context)
        {
            _context = context;
        }

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

        public bool IsWorkRegisterd(string workKey)
        {
            return _items.Any(x => x.WorkKey == workKey);
        }

        public void Add(WorkItem item)
        {
            if (IsWorkRegisterd(item.WorkKey))
            {
                throw new InvalidOperationException("Work with key " + item.WorkKey + " already registered.\r\n" 
                    + "You can use IsWorkRegisterd to check if work is already registered");
            }
            _items.Add(item);
        }        
    }
}
