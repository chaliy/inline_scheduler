using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace InlineScheduler.Advanced
{
    public class WorkBag : IEnumerable<WorkDef>
    {
        private readonly ConcurrentBag<WorkDef> _defs = new ConcurrentBag<WorkDef>();

        public IEnumerator<WorkDef> GetEnumerator()
        {
            return _defs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IList<WorkDef> GetApplicableToRun(int page)
        {
            return _defs
                    .Reverse() // WTF? This is temporary, until we do not have priority stuff
                    .Where(x => x.Status == WorkStatus.Scheduled)
                    .Take(page)
                    .ToList();
        }

        public void UpdateScheduledStatus() 
        {
            foreach(var def in _defs.Where(x => x.Status == WorkStatus.Pending))
            {
                def.UpdateScheduledStatus();                
            }
        }

        public int GetRuningWork()
        {
            return _defs.Count(x => x.Status == WorkStatus.Running);                    
        }

        public void Add(string workKey, Func<Task> factory, TimeSpan interval)
        {
            if (!_defs.Any(x => x.WorkKey == workKey))
            {                
                _defs.Add(new WorkDef(workKey, factory)
                              {
                                  Interval = interval
                              });
            }
        }        
    }
}
