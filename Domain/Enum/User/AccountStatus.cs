using System.ComponentModel;

namespace Event_Management.Domain.Enum
{
    public enum AccountStatus
    {
        [Description("All")]
        All = 0,
        [Description("Active")]
        Active = 1,
        [Description("Pending")]
        Pending = 2,
        [Description("Blocked")]
        Blocked = 3
    }
}
