using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum.Events
{
    public enum EventStatus
    {
        OnGoing,
        Ended,
        NotYet,
        Aborted,
        Deleted,
        Cancel
    }
}
