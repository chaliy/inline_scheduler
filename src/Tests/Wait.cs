using System;
using System.Threading;

namespace InlineScheduler.Tests
{
    static class Wait
    {
        public static void Until(Func<bool> check, int tenthsofsecondstowait)
        {
            for (var i = 0; i < tenthsofsecondstowait; i++)
            {
                if (check())
                {
                    break;
                }
                Thread.Sleep(100);
            }
        }

        public static void Until(Func<bool> check) 
        {
            Until(check, 1000);
        }

        public static void For(TimeSpan timeOut)
        {
            Thread.Sleep(timeOut);
        }
    }
}
