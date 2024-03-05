namespace RollBackManagerLab.Web.Constants
{
    public static class ErrorMessageContants
    {
        public const string Key = "error";
        public const string EmailAndPasswordEmptyErrorMessage = "Please enter email address and password.";
        public const string EmptyCaptchaErrorMessage = "Please enter captcha code.";
        public const string EmailRegexNotValideErrorMessage = "Please enter valid email address.";
        public const string CaptchaIsNotExpectedValueErrorMessage = "Please enter valid captcha code.";
        public const string InvalidAuthErrorMessage = "Invalid authentication. Check your email address and password.";
        public const string InvalidOtpCode = "Invalid Verification Code.";
        public const string NotFoundOtpCode = "Verification Code Not Found Or Expired Session.Please Retry Attempt.";
        public const string NotFoundRegistration = "Registration Not Found Or Expired Session.Please Retry Attempt.";
        public const string RegistrationFailed = "Registration Failed. Please Try Again.";
        public const string PasswordRuleErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 8 characters long.";
    }
}
