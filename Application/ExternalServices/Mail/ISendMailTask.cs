using Domain.DTOs.ParticipantDto;
using Domain.DTOs.User.Request;

namespace Application.ExternalServices.Mail
{
    public interface ISendMailTask
    {
        void SendMailTicket(RegisterEventModel registerEventModel);
        void SendMailReminder(Guid eventId);
        void SendMailVerify(UserMailDto userMailDto);
    }
}
