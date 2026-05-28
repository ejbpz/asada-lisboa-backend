namespace AsadaLisboaBackend.Utils
{
    public static class Constants
    {
        public const int PAGINATION_SIZE = 8;
        public const string CONTACT_EMAIL = "CONTACT_EMAIL";
        public const string RESEND_API_TOKEN = "RESEND_API_TOKEN";

        public const string DOMAIN_HOST = "https://asadalisboa.org";
        public const string CLIENT_HOST = "https://asadalisboa.org";

        public const string PHONE_REGEX = @"^(?:\d{8}|\d{4}-\d{4}|(?:\d{2}-){3}\d{2})$";
        public const string EMAIL_REGEX = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
        public const string PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&\.*]).{8,}$";

        public const string DOMAIN_RECAPTCHA = "https://challenges.cloudflare.com/turnstile/v0/siteverify";

        public const string ROLE_LECTOR = "Lector";
        public const string ROLE_ESCRITOR = "Escritor";
        public const string ROLE_ADMINISTRADOR = "Administrador";

        public const string CACHE_NEWS = "news";
        public const string CACHE_USERS = "users";
        public const string CACHE_ROLES = "roles";
        public const string CACHE_IMAGES = "images";
        public const string CACHE_CHARGES = "charges";
        public const string CACHE_ABOUT_US = "about_us";
        public const string CACHE_STATUSES = "statuses";
        public const string CACHE_CONTACTS = "contacts";
        public const string CACHE_DOCUMENTS = "documents";
        public const string CACHE_PRINCIPALS = "principals";
        public const string CACHE_CATEGORIES = "categories";
        public const string CACHE_CONFIGURATIONS = "configurations";
    }
}
