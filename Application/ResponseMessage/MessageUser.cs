namespace Application.ResponseMessage
{
    public static class MessageUser
    {
        //User Message response
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string LoginFailed = "INCORRECT_USERNAME_OR_PASSWORD!";
        public const string LoginSuccess = "LOGIN_SUCCESSFULLY";
        public const string RegisterSuccess = "REGISTER_SUCCESSFULLY";
        public const string RegisterFailed = "REGISTER_FAIL";
        public const string ValidateSuccessfully = "VALIDATE_SUCCESSFULLY";
        public const string ValidateFailed = "VALIDATE_FAILED";
        public const string LogoutSuccess = "LOG_OUT_SUCCESSFULLY";
        public const string OTPSuccess = "OTP_SENT_SUCCESSFULLY";
        public const string OTPNotFound = "OTP_NOT_FOUND";
        public const string OTPExpired = "OTP_EXPIRED";
        public const string PhoneExisted = "EXISTED_PHONE";

        //Authentication Message response
        public const string TokenInvalid = "INVALID_TOKEN";
        public const string TokenExpired = "TOKEN_EXPIRED";
        public const string TokenRefreshSuccess = "REFRESH_SUCCESSFULLY";
    }
}
