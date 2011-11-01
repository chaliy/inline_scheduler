using System;
using System.Threading.Tasks;
namespace InlineScheduler.Advanced
{
    public class WorkItemFactory
    {
        private readonly IWorkContext _context;

        public WorkItemFactory(IWorkContext context)
        {
            _context = context;
        }

        public WorkItem Create(string workKey, Func<Task> factory, TimeSpan interval, string description = null)
        {
            var stateProvider = _context.State;
            var state = stateProvider.Retrieve(workKey); // State could be null!                

            return new WorkItem(_context, workKey, factory, interval, state)
            {
                Description = description
            };
        }
    }
}
