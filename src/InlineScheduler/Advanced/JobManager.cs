using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace InlineScheduler.Advanced
{
    public class JobManager
    {
        private readonly ISchedulerContext _context;
        readonly ConcurrentDictionary<string, JobItem> _items
            = new ConcurrentDictionary<string, JobItem>();

        public JobManager(ISchedulerContext context)
        {
            _context = context;
        }

        public IEnumerable<JobItem> GetAll()
        {
            return _items.Values;
        }
        
        public IList<JobItem> GetApplicableToRun(int page)
        {
            return _items.Values
                    .Reverse() // WTF? This is temporary, until we do not have priority stuff
                    .Where(x => x.Status == JobStatus.Scheduled)
                    .Take(page)
                    .ToList();
        }

        public void UpdateState() 
        {
            foreach(var item in _items.Values)
            {
                item.UpdateState();                
            }
        }

        public int GetRuningWork()
        {
            return _items.Values.Count(x => x.Status == JobStatus.Running);
        }

        public bool IsJobRegisterd(string jobKey)
        {
            return _items.ContainsKey(jobKey);
        }

        [Obsolete]
        public void Add(JobItem item)
        {
            if (IsJobRegisterd(item.JobKey))
            {
                throw new InvalidOperationException("Job with key " + item.JobKey + " already registered.\r\n"
                    + "You can use IsJobRegisterd to check if job is already registered");
            }
            _items.TryAdd(item.JobKey, item);            
        }

        public void Register(JobDefinition def)
        {
            if (IsJobRegisterd(def.JobKey))
            {
                throw new InvalidOperationException("Job with key " + def.JobKey + " already registered.\r\n"
                    + "You can use IsJobRegisterd to check if job is already registered");
            }
            _items.TryAdd(def.JobKey, new JobItemFactory(_context).Create(def));
        }

        public void Force(string jobKey)
        {
            var def = _items[jobKey];
            if (def != null)
            {
                def.Force();
            }
        }
    }
}
