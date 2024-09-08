using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Quartz
{
    public class EventStartingEmailJob
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ISendMailTask _sendMailTask;
    }
}
