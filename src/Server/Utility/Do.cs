using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace InlineScheduler.Server.Utility
{
    static class Do
    {
        public static void TryWaitUntil(Func<bool> check, TimeSpan timeout) 
        {
            var iterations = timeout.TotalMilliseconds / 200;
            for (var i = 0; i < iterations; i++)
            {
                if (!check()) 
                {
                    return;
                }
                Thread.Sleep(200);
            }            
        }
    }
}
