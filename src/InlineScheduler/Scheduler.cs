using System;
using System.Linq;
using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System.Threading;

namespace InlineScheduler
{
    public class Scheduler
    {
        private readonly JobManager _work;        
        private readonly DateTime _sartTime;

        private bool _stopped;

        private readonly Timer _timer;

        public Scheduler(ISchedulerContext context = null)
        {
            context = context ?? new DefaultSchedulerContext();
            _work = new JobManager(context);            
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
        
        public SchedulerStats GatherStats(string filter = null)
        {
            return new SchedulerStats
            {
                Overal = GatherOveralStats(),
                CurrentJobs = StatsHelper.GatherCurrentJobs(_work.GetAll(), filter)
            };
        }

        public OveralStats GatherOveralStats()
        {
            var stats = StatsHelper.GatherOveralStatistics2(_work.GetAll());
            stats.IsStopped = _stopped;
            stats.StartTime = _sartTime;
            return stats;            
        }

        public SchedulerJobStats GatherJobStats(string workKey)
        {
            return StatsHelper.GatherJobStats(_work.GetAll(), workKey);
        }

        public bool IsStopped { get { return _stopped; } }
        public bool IsRunningJobsNow
        {
            get
            {
                return GatherOveralStats().RunningJobs > 0;
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
            var definition = new JobDefinition(workKey, factory, Schedules.Interval(interval), description);
            Schedule(definition);
        }

        public void Schedule(string workKey, Func<Task> factory, TimeSpan interval, string description = null)
        {
            var definition = new JobDefinition(workKey, factory, Schedules.Interval(interval), description);
            Schedule(definition);
        }
        
        public void Schedule(JobDefinition definition)
        {
            if (!_work.IsJobRegisterd(definition.JobKey))
            {
                _work.Register(definition);
            }
        }

        /// <summary>
        ///     Forces job to run.
        /// </summary>
        public void Force(string jobKey)
        {
            _work.Force(jobKey);
        }        
    }
}
