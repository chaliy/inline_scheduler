using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InlineScheduler.Advanced
{
    public class WorkDef
    {
        private readonly string _workKey;
        private readonly Func<Task> _factory;

        private WorkStatus _status;
        private bool _forced;
        private DateTime? _lastStart;
        private DateTime? _lastComplete;
        private readonly List<WorkRun> _previousRuns = new List<WorkRun>();

        public WorkDef(string workKey, Func<Task> factory)
        {
            _workKey = workKey;
            _factory = factory;
            Interval = TimeSpan.FromMinutes(2);
        }

        public string WorkKey { get { return _workKey; } }
        public Func<Task> Factory { get { return _factory; } }

        public TimeSpan Interval { get; set; }
        public WorkStatus Status { get { return _status; } }
        public DateTime? LastStart { get { return _lastStart; } }
        public DateTime? LastComplete { get { return _lastComplete; } }
        public List<WorkRun> PreviousRuns { get { return _previousRuns; } }

        public bool IsApplicableToRun
        {
            get
            {
                return Status == WorkStatus.Pending
                    && (DateTime.Now > (_lastComplete.GetValueOrDefault() + Interval) || _forced);
            }
        }

        public void Run()
        {
            if (_status != WorkStatus.Pending) 
            {
                // Do nothing. Work is already started.
                return;
            }
            _status = WorkStatus.Scheduled;
            Task.Factory.StartNew(() => 
            {
                _status = WorkStatus.Running;
                _lastStart = DateTime.Now;
                return Factory().ContinueWith(t =>
                {
                    _status = WorkStatus.Pending;
                    _forced = false;
                    _lastComplete = DateTime.Now;

                    _previousRuns.Add(new WorkRun
                    {
                        Started = _lastStart.Value,
                        Completed = _lastComplete.Value
                    });

                    if (t.Status == TaskStatus.Faulted)
                    {
                        var ex = t.Exception.Flatten().GetBaseException();
                        // We need to log this out.
                        //_trace.Value.Error("Command " + cmdKey + " failed to execute.\r\n" + ex.Message, new { Exception = ex, Command = cmd });
                        //throw ex;
                    }
                    else if (t.Status == TaskStatus.RanToCompletion)
                    {
                    }
                });
            }, TaskCreationOptions.LongRunning)
            ;
        }

        public void Force()
        {
            _forced = true;
        }
    }
}
