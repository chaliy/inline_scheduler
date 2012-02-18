using System;
using System.Threading.Tasks;

namespace InlineScheduler.Advanced
{
    public class JobDefinition
    {
        readonly string _jobKey;
        readonly Func<Task> _factory;
        readonly IJobSchedule _schedule;
        readonly string _description;

        public JobDefinition(string jobKey, Func<Task> factory, IJobSchedule schedule = null, string description = null)
        {
            _jobKey = jobKey;
            _factory = factory;
            _schedule = schedule;
            _description = description;
        }

        public string JobKey
        {
            get { return _jobKey; }
        }

        public Func<Task> Factory
        {
            get { return _factory; }
        }

        public IJobSchedule Schedule
        {
            get { return _schedule; }
        }

        public string Description
        {
            get { return _description; }
        }
    }
}
