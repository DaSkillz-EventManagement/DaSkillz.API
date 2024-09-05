namespace Application.ResponseMessage
{
    public static class MessageUser
    {
        //User Message response
        public const string UserNotFound = "User not found!";
        public const string LoginFailed = "Username or password incorrect!";
        public const string LoginSuccess = "Login successfully";
        public const string RegisterSuccess = "User registered successfully";
        public const string RegisterFailed = "Failed to register user";
        public const string ValidateSuccessfully = "validate successfully";
        public const string ValidateFailed = "validate failed";
        public const string LogoutSuccess = "Logged out successfully";
        public const string OTPSuccess = "OTP sent successfully";
        public const string OTPNotFound = "OTP not found";
        public const string OTPExpired = "OTP expired";
        public const string PhoneExisted = "Phone has already existed";

        //Authentication Message response
        public const string TokenInvalid = "Invalid token";
        public const string TokenExpired = "Tokens expired";
        public const string TokenRefreshSuccess = "Token refreshed successfully";
    }
}
