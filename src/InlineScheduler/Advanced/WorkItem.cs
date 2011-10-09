using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InlineScheduler.Advanced
{
    public class WorkItem
    {
        private readonly WorkContext _context;
        private readonly string _workKey;
        private readonly Func<Task> _factory;

        private WorkStatus _status;        
        private DateTime? _lastStart;
        private DateTime? _lastComplete;
        private readonly List<WorkRun> _previousRuns = new List<WorkRun>();        

        public WorkItem(WorkContext context, string workKey, Func<Task> factory)
        {
            _context = context;
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
        
        public void Run()
        {
            if (_status != WorkStatus.Scheduled) 
            {
                // Do nothing. Work is already started.
                return;
            }            
            _status = WorkStatus.Running;
            _lastStart = _context.CurrentTime;
            Factory().ContinueWith(t =>
            {
                _status = WorkStatus.Pending;
                _lastComplete = _context.CurrentTime;

                var run = new WorkRun
                                {
                                    Started = _lastStart.Value,
                                    Completed = _lastComplete.Value
                                };
                    
                if (t.Status == TaskStatus.Faulted)
                {                        
                    var ex = t.Exception.Flatten().GetBaseException();

                    run.Result = WorkRunResult.Failure;
                    run.ResultMessage = ex.ToString();

                    // We need to log this out.
                    //_trace.Value.Error("Command " + cmdKey + " failed to execute.\r\n" + ex.Message, new { Exception = ex, Command = cmd });
                }
                else if (t.Status == TaskStatus.RanToCompletion)
                {
                    run.Result = WorkRunResult.Success;
                }

                _previousRuns.Add(run);
            });                         
        }

        public void Force()
        {
            if (_status == WorkStatus.Pending
                || _status == WorkStatus.Holded)
            {
                _status = WorkStatus.Scheduled;
            }
        }

        public void UpdateState()
        {
            if (_status == WorkStatus.Pending)
            {
                if (_context.CurrentTime > (_lastComplete.GetValueOrDefault() + Interval))
                {
                    _status = WorkStatus.Scheduled;
                }
            }

            if (_previousRuns.Count > 5) 
            {
                _previousRuns.RemoveRange(0, _previousRuns.Count - 5);
            }
        }        
    }
}
