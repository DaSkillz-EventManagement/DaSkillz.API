namespace Application.ResponseMessage
{
    public class MessageParticipant
    {
        public const string CheckInUserSuccess = "Check in success!";
        public const string CheckInUserFailed = "QR Code not valid!";
        public const string NotOwner = "You are not owner of this event";
        public const string ProcessParticipant = "Process participant success!";
        public const string ProcessParticipantFailed = "Process participant failed!";
        public const string ExistedOnEvent = "You already registered on event!";
        public const string AcceptInvite = "Accept invite success!";
        public const string AcceptInviteFailed = "Accept invite failed!";
        public const string HostCannotRegister = "Host cannot register on event!";
        public const string ParticipantStatusNotValid = "Participant status not valid!";
        public const string YouAreNotStaff = "You are not staff of this event!";
        public const string TransactionIsNotValid = "Transaction is not valid! Please, confirm pay out success!";
        public const string ParticipantCapacityLimitReached = "Participant capacity limit reached! Cannot add new participant!";
    }
}
