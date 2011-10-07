using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
//using System.Threading.Tasks.Dataflow;
using System.Collections.Generic;
using InlineScheduler;

namespace SimpleConsoleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new Scheduler();
            var random = new Random();
            for (var i = 0; i < 1000; i++)
            {
                var id = "Foo" + i;
                scheduler.Schedule(id, () =>
                {                    
                    return Task.Factory.StartNew(() =>
                    {
                        Console.WriteLine(id + " Started");
                        for (int a = 0; a < 10000 + random.Next(); a++)
                        {
                            Thread.Sleep(1);
                        }
                        Console.WriteLine(id + " Completed");
                    });
                });
            }
            
            Console.WriteLine("S - to get starts, enything else to exit.");
            while(true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.S)
                {
                    Console.WriteLine("tats>> " + scheduler.Stats);
                }
                else
                {
                    break;
                }
            }
        }
    }   
}
