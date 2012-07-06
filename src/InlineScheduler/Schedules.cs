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

        public static IJobSchedule Daily(TimeSpan daytime) {
            return new DailyJobSchedule(daytime);
        }
    }

    public static class JobFactory
    {

        public static JobDefinition Schedule(string jobKey, Action exe, IJobSchedule schedule, string description = null) {
            Func<Task> factory = () => Task.Factory.StartNew(exe);
            return new JobDefinition(jobKey, factory, schedule, description);
        }

        public static JobDefinition Schedule(string jobKey, Func<Task> factory, IJobSchedule schedule, string description = null) {
            return new JobDefinition(jobKey, factory, schedule, description);
        }

        public static JobDefinition Interval(string jobKey, Action exe, TimeSpan interval, string description = null)
        {
            return Schedule(jobKey, exe, Schedules.Interval(interval), description);
        }

        public static JobDefinition Interval(string jobKey, Func<Task> factory, TimeSpan interval, string description = null)
        {
            return new JobDefinition(jobKey, factory, Schedules.Interval(interval), description);
        }

        public static JobDefinition Daily(string jobkey, Action exe, TimeSpan daytime, string description = null) {
            return Schedule(jobkey, exe, Schedules.Daily(daytime), description);
        }
    }
}
