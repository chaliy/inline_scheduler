using System;

namespace InlineScheduler.Advanced
{
    static class DateTimeExtensions
    {
        public static bool WithInMinute(this DateTime @this, DateTime? other)
        {
            return other < @this.AddMinutes(1);
        }
    }
}
