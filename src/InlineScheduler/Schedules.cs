using System;
using System.Threading.Tasks;
using InlineScheduler.Advanced;

namespace InlineScheduler
{
    public static class Schedules
    {
        public static IJobSchedule Interval(TimeSpan interval)
        {
            return new IntervalJobSchedule(interval);
        }
    }

    public static class JobFactory
    {
        public static JobDefinition Interval(string jobKey, Action exe, TimeSpan interval, string description = null)
        {
            Func<Task> factory = () => Task.Factory.StartNew(exe);
            return new JobDefinition(jobKey, factory, Schedules.Interval(interval), description);            
        }

        public static JobDefinition Interval(string jobKey, Func<Task> factory, TimeSpan interval, string description = null)
        {
            return new JobDefinition(jobKey, factory, Schedules.Interval(interval), description);
        }
    }
}
