using System;
using System.Threading;

namespace InlineScheduler.Tests
{
    static class Wait
    {
        public static void Unitl(Func<bool> check)
        {
            for (var i = 0; i < 1000; i++)
            {
                if (check())
                {
                    break;
                }
                Thread.Sleep(100);
            }
        }

        public static void For(TimeSpan timeOut)
        {
            Thread.Sleep(timeOut);
        }
    }
}
