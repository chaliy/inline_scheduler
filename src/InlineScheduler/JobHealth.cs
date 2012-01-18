using System.Runtime.Serialization;

namespace InlineScheduler
{
    public enum JobHealth
    {
        [EnumMember(Value = "Good")]
        Good,
        [EnumMember(Value = "Bad")]
        Bad
    }
}