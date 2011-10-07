using System;
using System.Linq;
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
                return StatsHelper.GatherOveralStatistics(_work);
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
                        Thread.Sleep(1000);
                    }
                }
            });
        }

        public void Schedule(string workKey, Func<Task> factory, TimeSpan interval)
        {
            _work.Add(workKey, factory, interval);
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
