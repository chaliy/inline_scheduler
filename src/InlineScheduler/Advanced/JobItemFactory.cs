namespace InlineScheduler.Advanced
{
    public class JobItemFactory
    {
        private readonly ISchedulerContext _context;

        public JobItemFactory(ISchedulerContext context)
        {
            _context = context;
        }

        public JobItem Create(JobDefinition definition)
        {
            var stateProvider = _context.State;
            var state = stateProvider.Retrieve(definition.JobKey); // State could be null!                

            return new JobItem(_context, definition, state);
        }        
    }
}
