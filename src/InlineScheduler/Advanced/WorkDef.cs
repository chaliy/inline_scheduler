using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlineScheduler.Advanced
{
    public class WorkDef
    {
        private readonly string _workKey;
        private readonly Func<Task> _factory;

        private WorkStatus _status;
        private bool _forced;
        private DateTime lastComplete;

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


        public bool IsApplicableToRun
        {
            get
            {
                return Status == WorkStatus.Pending
                    && (DateTime.Now > (lastComplete + Interval)
                        || _forced);
            }
        }

        public void Run()
        {
            _status = WorkStatus.Running;
            Factory()
                .ContinueWith(t =>
                {
                    _status = WorkStatus.Pending;
                    _forced = false;
                    lastComplete = DateTime.Now;

                    if (t.Status == TaskStatus.Faulted)
                    {
                        var ex = t.Exception.Flatten().GetBaseException();
                        //_trace.Value.Error("Command " + cmdKey + " failed to execute.\r\n" + ex.Message, new { Exception = ex, Command = cmd });
                        //throw ex;
                    }
                    else if (t.Status == TaskStatus.RanToCompletion)
                    {
                    }
                });
        }

        public void Force()
        {
            _forced = true;
        }
    }
}
