namespace AsadaLisboaBackend.Utils
{
    public static class Constants
    {
        //public const string DOMAIN_HOST = "https://asadalisboa.or.cr"; // TODO: add domain
        public const int PAGINATION_SIZE = 8;
        public const string CONTACT_EMAIL = "CONTACT_EMAIL";
        public const string RESEND_API_TOKEN = "RESEND_API_TOKEN";
        public const string DOMAIN_HOST = "https://localhost:5199";
        public const string EMAIL_REGEX = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";
        public const string PHONE_REGEX = @"^(?:\d{8}|\d{4}-\d{4}|(?:\d{2}-){3}\d{2})$";
        public const string DOMAIN_RECAPTCHA = "https://www.google.com/recaptcha/api/siteverify";
    }
}
