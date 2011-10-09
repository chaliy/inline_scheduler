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
        private bool _stopped;

        public SchedulerStats Stats
        {
            get
            {
                var stats = StatsHelper.GatherOveralStatistics(_work);
                stats.IsStopped = _stopped;
                return stats;
            }
        }

        public Scheduler()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _work.UpdateScheduledStatus();
                    if (!_stopped)
                    {
                        var runningCount = _work.GetRuningWork();

                        if (runningCount < 20)
                        {
                            var applicableDefs = _work.GetApplicableToRun(20);

                            foreach (var def in applicableDefs)
                            {
                                def.Run();
                            }
                        }
                    }
                    
                    Thread.Sleep(1000);
                }
            });
        }

        public bool IsStopped { get { return _stopped; } }

        public void Stop() 
        {
            _stopped = true;
        }

        public void Start()
        {
            _stopped = false;
        }
        
        public void Schedule(string workKey, Func<Task> factory, TimeSpan interval)
        {
            _work.Add(workKey, factory, interval);
        }

        /// <summary>
        ///     Forces work item to run.
        /// </summary>
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
