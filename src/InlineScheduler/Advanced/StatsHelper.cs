using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InlineScheduler.Advanced
{
    public static class StatsHelper
    {
        public static OveralStats GatherOveralStatistics2(IEnumerable<JobItem> work)
        {           
            var jobs = work                
                .Select(x => new
                {                    
                    CurrentStatus = (InlineScheduler.JobStatus)x.Status                    
                }).ToList();

            return new OveralStats
            {
                PendingJobs = jobs.Count(x => x.CurrentStatus == InlineScheduler.JobStatus.Pending),
                RunningJobs = jobs.Count(x => x.CurrentStatus == InlineScheduler.JobStatus.Running),
                ScheduledJobs = jobs.Count(x => x.CurrentStatus == InlineScheduler.JobStatus.Scheduled),
                JobsCount = jobs.Count
            };
        }

        public static List<MinimalJobStats> GatherCurrentJobs(IEnumerable<JobItem> work, string filter)
        {
            return work
                .Reverse()
                .Where(x => 
                {
                    if (String.IsNullOrWhiteSpace(filter) || filter == "all")
                    {
                        return true;
                    }
                    if (filter == "running" && x.Status == JobStatus.Running) 
                    {
                        return true;
                    }
                    if (filter == "failing" && GatherJobHealth(x) == JobHealth.Bad)
                    {
                        return true;
                    }
                    return false;
                })
                .Select(x => new MinimalJobStats
                {
                    JobKey = x.JobKey,
                    Description = x.Description,
                    CurrentStatus = (InlineScheduler.JobStatus)x.Status,
                    Health = GatherJobHealth(x),
                    Report = GatherJobReport(x)
                }).ToList();
        }

        private static string GatherJobReport(JobItem jobItem)
        {
            if (jobItem.PreviousRuns.Count == 0)
            {
                return "N/A";
            }

            var message = new StringBuilder();

            var average = Math.Round(jobItem.PreviousRuns.Average(x => (x.Completed - x.Started).TotalSeconds), 2);
            message.AppendLine("Average execution time: " + average + "s.");

            var failedRuns = jobItem.PreviousRuns.Where(x => x.Result == JobRunResult.Failure).ToList();
            if (failedRuns.Count != 0)
            {
                message.AppendLine("Job failed at least " + failedRuns.Count + " times.");
            }

            return message.ToString();
        }

        private static JobHealth GatherJobHealth(JobItem x)
        {
            return x.PreviousRuns.Any(xx => xx.Result == JobRunResult.Failure) 
                ? JobHealth.Bad 
                : JobHealth.Good;
        }

        public static SchedulerJobStats GatherJobStats(IEnumerable<JobItem> work, string workKey)
        {
            return work
                .Where(x => x.JobKey == workKey)
                .Select(x => new SchedulerJobStats
                {
                    JobKey = x.JobKey,
                    Description = x.Description,
                    CurrentStatus = (InlineScheduler.JobStatus) x.Status,
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
