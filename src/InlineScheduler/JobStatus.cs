using System.Runtime.Serialization;

namespace InlineScheduler
{
    public enum JobStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Holded")]
        Holded,
        [EnumMember(Value = "Scheduled")]
        Scheduled,
        [EnumMember(Value = "Running")]
        Running
    }
}
