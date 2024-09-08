using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ExternalServices.Quartz
{
    public interface IQuartzService
    {
        Task StartEventStatusToOngoingJob(Guid eventId, DateTime startTime);
        Task StartEventStatusToEndedJob(Guid eventId, DateTime startTime);
        Task StartEventStartingEmailNoticeJob(Guid eventId, DateTime startTime);
        Task StartEventEndingEmailNoticeJob(Guid eventId, DateTime endTime);
        Task DeleteJobsByEventId(string eventId);
        Task<IEnumerable<string>> GetAllJob();
        Task ExcuteJob(string key, int type);
    }
}
