using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InlineScheduler.Advanced.State;

namespace InlineScheduler.Advanced
{
    public class JobItem : IJobState
    {
        readonly ISchedulerContext _context;
        readonly JobDefinition _definition;
        
        JobStatus _status;
        DateTime? _lastStartTime;
        DateTime? _lastCompleteTime;
        DateTime? _nextTime;

        readonly DateTime _created;        
        readonly List<JobRun> _previousRuns = new List<JobRun>();

        public JobItem(ISchedulerContext context, JobDefinition definition, WorkState presavedState = null)
        {
            _context = context;
            _definition = definition;
            _created = _context.GetCurrentTime();

            if (presavedState != null)             
            {
                _lastCompleteTime = presavedState.LastCompleteTime;
            }

            UpdateState();
        }

        public string JobKey { get { return _definition.JobKey; } }

        public string Description { get { return _definition.Description; } }
        public JobStatus Status { get { return _status; } }
        public DateTime Created { get { return _created; } }
        public DateTime? LastStart { get { return _lastStartTime; } }
        public DateTime? LastComplete { get { return _lastCompleteTime; } }
        public List<JobRun> PreviousRuns { get { return _previousRuns; } }        
        
        public void Run()
        {
            if (_status != JobStatus.Scheduled) 
            {
                // Do nothing. Work is already started.
                return;
            }            
            _status = JobStatus.Running;
            _lastStartTime = _context.GetCurrentTime();
            _definition.Factory().ContinueWith(t =>
            {
                _status = JobStatus.Pending;
                _lastCompleteTime = _context.GetCurrentTime();

                var run = new JobRun
                {
                    Started = _lastStartTime.Value,
                    Completed = _lastCompleteTime.Value
                };
                    
                if (t.Status == TaskStatus.Faulted)
                {                        
                    var ex = t.Exception.Flatten().GetBaseException();

                    run.Result = JobRunResult.Failure;
                    run.ResultMessage = ex.ToString();

                    // We need to log this out.
                    //_trace.Value.Error("Command " + cmdKey + " failed to execute.\r\n" + ex.Message, new { Exception = ex, Command = cmd });
                }
                else if (t.Status == TaskStatus.RanToCompletion)
                {
                    run.Result = JobRunResult.Success;
                }

                _previousRuns.Add(run);

                var newWorkState = new WorkState(_lastCompleteTime.Value);

                _context.State.Store(_definition.JobKey, newWorkState);
            });                         
        }

        public void Force()
        {
            if (_status == JobStatus.Pending
                || _status == JobStatus.Holded)
            {
                _status = JobStatus.Scheduled;
            }
        }

        public void UpdateState()
        {
            _nextTime = _definition.Schedule.NextExecution(this, _context);            
            if (_status == JobStatus.Pending)
            {
                if (_nextTime != null && _context.GetCurrentTime() >=_nextTime)
                {
                    _status = JobStatus.Scheduled;
                }
            }

            if (_previousRuns.Count > 5) 
            {
                _previousRuns.RemoveRange(0, _previousRuns.Count - 5);
            }
        }
    }
}
