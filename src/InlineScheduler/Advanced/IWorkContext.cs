using System;
namespace InlineScheduler.Advanced
{
    public interface IWorkContext
    {
        DateTime CurrentTime { get; }
    }
}
