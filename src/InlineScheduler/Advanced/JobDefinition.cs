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

    public interface IJobState
    {
        JobStatus Status { get; }
        DateTime Created { get; }
        DateTime? LastStart { get; }
        DateTime? LastComplete { get; }
    }

    public interface IJobSchedule
    {
        DateTime? NextExecution(IJobState state, ISchedulerContext context);
    }

    public class IntervalJobSchedule : IJobSchedule
    {
        readonly TimeSpan _interval;

        public IntervalJobSchedule(TimeSpan interval)
        {
            _interval = interval;
        }

        public DateTime? NextExecution(IJobState state, ISchedulerContext context)
        {
            if (state.Status != JobStatus.Pending)
            {
                return null; // Have no clue
            }

            if (state.LastComplete != null)
            {
                return state.LastComplete + _interval;                
            }
            else
            {
                var shift = context.GetNextRandom(200, (int) _interval.TotalMilliseconds/3);
                return state.Created.AddMilliseconds(shift);
            }
        }
    }
}
