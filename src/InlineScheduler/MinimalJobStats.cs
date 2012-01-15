namespace InlineScheduler
{
    public class MinimalJobStats
    {
        public string WorkKey { get; set; }
        public string Description { get; set; }
        public JobStatus CurrentStatus { get; set; }
        public JobHealth Health { get; set; }
        public string Report { get; set; }
    }
}
