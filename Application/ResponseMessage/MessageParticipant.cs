namespace Application.ResponseMessage
{

    public static class MessageEvent
    {
        // Event Message response
        public const string CheckInUserSuccess = "CHECK_IN_SUCCESS";
        public const string CheckInUserFailed = "INVALID_QR_CODE";
        public const string NotOwner = "NOT_EVENT_OWNER";
        public const string ProcessParticipant = "PROCESS_PARTICIPANT_SUCCESS";
        public const string ProcessParticipantFailed = "PROCESS_PARTICIPANT_FAILED";
        public const string ExistedOnEvent = "ALREADY_REGISTERED_ON_EVENT";
        public const string AcceptInvite = "ACCEPT_INVITE_SUCCESS";
        public const string AcceptInviteFailed = "ACCEPT_INVITE_FAILED";
        public const string HostCannotRegister = "HOST_CANNOT_REGISTER";
        public const string ParticipantStatusNotValid = "INVALID_PARTICIPANT_STATUS";
        public const string YouAreNotStaff = "NOT_EVENT_STAFF";

        //"Transaction is not valid! Please, confirm pay out success!";
        public const string TransactionIsNotValid = "INVALID_TRANSACTION_CONFIRM_PAYMENT";
        public const string ParticipantCapacityLimitReached = "PARTICIPANT_CAPACITY_LIMIT_REACHED";
    }


}
