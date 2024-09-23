using Domain.Enum.Participant;
using Event_Management.Domain.Enum;

namespace Application.Helper
{
    public class Utilities
    {
        public static string GetTypeButton(int eventRole)
        {
            switch (eventRole)
            {
                case 1:
                case 3:
                    return "Approval";
                case 4:
                case 0:
                case 5:
                    return "My Ticket";
            }

            return string.Empty;
        }

        public static ParticipantStatus GetParticipantStatus(string status)
        {
            if (ParticipantStatus.Pending.ToString().Equals(status))
            {
                return ParticipantStatus.Pending;
            }
            else if (ParticipantStatus.Confirmed.ToString().Equals(status))
            {
                return ParticipantStatus.Confirmed;
            }
            else if (ParticipantStatus.Blocked.ToString().Equals(status))
            {
                return ParticipantStatus.Blocked;
            }
            else if (ParticipantStatus.Cancel.ToString().Equals(status))
            {
                return ParticipantStatus.Cancel;
            }


            throw new Exception("Status not found");
        }

        public static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
        public static bool IsValidParticipantStatus(string status)
        {
            // Attempt to parse the string as a ParticipantStatus enum value
            return Enum.TryParse<ParticipantStatus>(status, out _);
        }

    }
}
