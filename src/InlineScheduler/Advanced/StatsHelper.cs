using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InlineScheduler.Advanced
{
    public static class StatsHelper
    {
        public static OveralStats GatherOveralStatistics2(IEnumerable<WorkItem> work)
        {           
            var jobs = work                
                .Select(x => new
                {                    
                    CurrentStatus = (JobStatus)x.Status                    
                }).ToList();

            return new OveralStats
            {
                PendingJobs = jobs.Count(x => x.CurrentStatus == JobStatus.Pending),
                RunningJobs = jobs.Count(x => x.CurrentStatus == JobStatus.Running),
                ScheduledJobs = jobs.Count(x => x.CurrentStatus == JobStatus.Scheduled),
                JobsCount = jobs.Count
            };
        }

        public static List<MinimalJobStats> GatherCurrentJobs(WorkBag work)
        {
            return work
                .Reverse()
                .Select(x => new MinimalJobStats
                {
                    WorkKey = x.WorkKey,
                    Description = x.Description,
                    CurrentStatus = (JobStatus)x.Status,
                    Health = GatherJobHealth(x),
                    Report = GatherJobReport(x)
                }).ToList();
        }

        private static string GatherJobReport(WorkItem workItem)
        {
            if (workItem.PreviousRuns.Count == 0)
            {
                return "N/A";
            }

            var message = new StringBuilder();

            var average = Math.Round(workItem.PreviousRuns.Average(x => (x.Completed - x.Started).TotalSeconds), 2);
            message.AppendLine("Average execution time: " + average + "s.");

            var failedRuns = workItem.PreviousRuns.Where(x => x.Result == WorkRunResult.Failure).ToList();
            if (failedRuns.Count != 0)
            {
                message.AppendLine("Job failed at least " + failedRuns.Count + " times.");
            }

            return message.ToString();
        }

        private static JobHealth GatherJobHealth(WorkItem x)
        {
            return x.PreviousRuns.Any(xx => xx.Result == WorkRunResult.Failure) 
                ? JobHealth.Bad 
                : JobHealth.Good;
        }

        public static SchedulerJobStats GatherJobStats(WorkBag work, string workKey)
        {
            return work
                .Where(x => x.WorkKey == workKey)
                .Select(x => new SchedulerJobStats
                {
                    WorkKey = x.WorkKey,
                    Description = x.Description,
                    CurrentStatus = (JobStatus) x.Status,
                    LastRunStarted = x.LastStart,
                    LastRunCompleted = x.LastComplete,
                    PreviousRuns = x.PreviousRuns.Select(xx => new SchedulerJobRunStats
                    {
                        Started = xx.Started,
                        Completed = xx.Completed,
                        Result = (SchedulerJobRunResult) xx.Result,
                        ResultMessage = xx.ResultMessage
                    }).ToList()
                })
                .FirstOrDefault();
        }
    }
}
