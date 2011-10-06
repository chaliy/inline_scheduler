using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SimpleConsoleExample
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Scheduler
    {
        //private readonly ConcurrentBag<WorkState> _state = new ConcurrentBag<WorkState>();
        private readonly ConcurrentBag<WorkDef>  _defs = new ConcurrentBag<WorkDef>();
        //private readonly ActionBlock<WorkItem> _queue = new ActionBlock<WorkItem>(i => i.Factory(), new ExecutionDataflowBlockOptions
        //       {
        //           MaxDegreeOfParallelism = 8
        //       });
        

        public void Start()
        {
            foreach (var def in _defs)
            {
                Task.Factory.StartNew(() =>
                {

                });
            }
        }

        //private class WorkState
        //{
        //    public string WorkKey { get; set; }
        //    public DateTime LastCompleteTime { get; set; }
        //}

        private class WorkDef
        {
            public Func<Task> Factory { get; set; }
            public string WorkKey { get; set; }
        }
        
        //private class WorkItem
        //{
        //    public Func<Task> Factory { get; set; }
        //    public string WorkKey { get; set; }
        //}
    }    
}
