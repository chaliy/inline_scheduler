using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System.Threading;

namespace InlineScheduler
{
    public class Scheduler
    {
        private readonly WorkBag _work = new WorkBag();

        public SchedulerStats Stats
        {
            get
            {
                var stats = new SchedulerStats();

                foreach (var group in _work.GroupBy(x => x.Status))
                {
                    switch (group.Key)
                    {
                        case WorkStatus.Pending:
                            stats.PendingJobs = group.Count();
                            break;

                        case WorkStatus.Running:
                            stats.RunningJobs = group.Count();
                            break;
                    }
                }

                return stats;
            }
        }

        public Scheduler()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var applicableDefs = _work.GetApplicableToRun();

                    foreach (var def in applicableDefs)
                    {
                        def.Run();
                    }

                    if (applicableDefs.Count == 0)
                    {
                        Thread.Sleep(10000);
                    }
                }
            });
        }

        public void Schedule(string workKey, Func<Task> factory)
        {
            _work.Add(workKey, factory);
        }

        public void Force(string workKey)
        {
            var def = _work.FirstOrDefault(x => x.WorkKey == workKey);
            if (def != null)
            {
                def.Force();
            }
        }
    }
}
