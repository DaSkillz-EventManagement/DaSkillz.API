using Domain.DTOs.ParticipantDto;
using Domain.DTOs.User.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ExternalServices.Mail
{
    public interface ISendMailTask
    {
        void SendMailTicket(RegisterEventModel registerEventModel);
        void SendMailReminder(Guid eventId);
        void SendMailVerify(UserMailDto userMailDto);
    }
}
