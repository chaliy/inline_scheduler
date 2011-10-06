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
            scheduler.Schedule("Foo1", () => {
                return Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Foo1 Started");
                    Thread.Sleep(10000);
                    Console.WriteLine("Foo1 Conpleted");
                });
            });

            scheduler.Schedule("Foo2", () =>
            {
                return Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Foo2 Started");
                    Thread.Sleep(30000);
                    Console.WriteLine("Foo2 Conpleted");
                });
            });


            Console.ReadLine();
        }
    }   
}
