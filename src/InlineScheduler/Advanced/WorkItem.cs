using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InlineScheduler.Advanced
{
    public class WorkItem
    {
        private readonly IWorkContext _context;
        private readonly string _workKey;        
        private readonly Func<Task> _factory;

        private WorkStatus _status;
        private DateTime? _lastStartTime;
        private DateTime? _lastCompleteTime;
        private DateTime _createdTime;
        private readonly List<WorkRun> _previousRuns = new List<WorkRun>();
        private readonly TimeSpan _interval;

        public WorkItem(IWorkContext context, string workKey, Func<Task> factory, TimeSpan interval)
        {
            _context = context;
            _workKey = workKey;
            _factory = factory;
            _createdTime = _context.CurrentTime;
            _interval = interval;
            UpdateState();
        }

        public string WorkKey { get { return _workKey; } }
        public Func<Task> Factory { get { return _factory; } }

        public TimeSpan Interval { get { return _interval; } }
        public string Description { get; set; }
        public WorkStatus Status { get { return _status; } }
        public DateTime? LastStart { get { return _lastStartTime; } }
        public DateTime? LastComplete { get { return _lastCompleteTime; } }
        public List<WorkRun> PreviousRuns { get { return _previousRuns; } }
        
        public void Run()
        {
            if (_status != WorkStatus.Scheduled) 
            {
                // Do nothing. Work is already started.
                return;
            }            
            _status = WorkStatus.Running;
            _lastStartTime = _context.CurrentTime;
            Factory().ContinueWith(t =>
            {
                _status = WorkStatus.Pending;
                _lastCompleteTime = _context.CurrentTime;

                var run = new WorkRun
                                {
                                    Started = _lastStartTime.Value,
                                    Completed = _lastCompleteTime.Value
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
                if (_context.CurrentTime > ((_lastCompleteTime ?? _createdTime) + Interval))
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
