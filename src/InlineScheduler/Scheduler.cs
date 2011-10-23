using System;
using System.Linq;
using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System.Threading;

namespace InlineScheduler
{
    public class Scheduler
    {
        private readonly WorkBag _work;
        private bool _stopped;
        private readonly DateTime _sartTime;

        private readonly Timer _timer;

        public Scheduler(IWorkContext context = null)
        {
            //HostingEnvironment
            _work = new WorkBag(context);
            _stopped = true;
            _sartTime = DateTime.Now;
            
            // Start timer
            _timer = new Timer(OnTimerElapsed);
            _timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));

        }

        void OnTimerElapsed(object sender)  
        {
            _work.UpdateState();
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
        }

        public SchedulerStats Stats
        {
            get
            {
                var stats = StatsHelper.GatherOveralStatistics(_work);
                stats.IsStopped = _stopped;
                stats.StartTime = _sartTime;
                return stats;
            }
        }

        public bool IsStopped { get { return _stopped; } }
        public bool IsRunningJobsNow
        {
            get
            {
                return Stats.RunningJobs > 0;
            }
        }

        public void Stop() 
        {
            _stopped = true;
        }

        public void Start()
        {
            _work.UpdateState();
            _stopped = false;
        }

        public void Schedule(string workKey, Action work, TimeSpan interval, string description = null)
        {
            Func<Task> factory = () => Task.Factory.StartNew(work);

            _work.Add(workKey, factory, interval, description);            
        }
        
        public void Schedule(string workKey, Func<Task> factory, TimeSpan interval, string description = null)
        {
            _work.Add(workKey, factory, interval, description);
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
