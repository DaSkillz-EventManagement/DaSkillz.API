using Domain.DTOs.ParticipantDto;

namespace Application.ExternalServices.Mail
{
    public interface ISendMailTask
    {
        void SendMailTicket(RegisterEventModel registerEventModel);
        void SendMailReminder(Guid eventId);
        //void SendMailVerify(UserMailDto userMailDto);
    }
}
