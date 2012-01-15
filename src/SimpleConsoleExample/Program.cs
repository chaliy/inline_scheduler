using System;
using System.Threading;
using System.Threading.Tasks;
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
                        for (var a = 0; a < 10000 + random.Next(); a++)
                        {
                            Thread.Sleep(1);
                        }
                        Console.WriteLine(id + " Completed");
                    });
                }, TimeSpan.FromMinutes(3));
            }
            
            Console.WriteLine("S - to get starts, anything else to exit.");
            while(true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.S)
                {
                    Console.WriteLine("tats>> " + scheduler.GatherOveralStats());
                }
                else
                {
                    break;
                }
            }
        }
    }   
}
