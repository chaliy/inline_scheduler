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

        public IList<WorkDef> GetApplicableToRun()
        {
            return _defs
                    .Reverse() // WTF? This is temporary, untill we do not have priority stuff
                    .Where(x => x.IsApplicableToRun)
                    .Take(20)
                    .ToList();
        }

        public void Add(string workKey, Func<Task> factory)
        {
            if (!_defs.Any(x => x.WorkKey == workKey))
            {                
                _defs.Add(new WorkDef(workKey, factory));
            }
        }        
    }
}
