namespace AsadaLisboaBackend.Utils
{
    public static class Constants
    {
        //public const string DOMAIN_HOST = "https://asadalisboa.or.cr"; // TODO: add domain
        public const int PAGINATION_SIZE = 8;
        public const string CONTACT_EMAIL = "CONTACT_EMAIL";
        public const string RESEND_API_TOKEN = "RESEND_API_TOKEN";
        public const string DOMAIN_HOST = "http://localhost:5199";
        public const string CLIENT_HOST = "http://localhost:4200";
        public const string EMAIL_REGEX = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";
        public const string PHONE_REGEX = @"^(?:\d{8}|\d{4}-\d{4}|(?:\d{2}-){3}\d{2})$";
        public const string DOMAIN_RECAPTCHA = "https://www.google.com/recaptcha/api/siteverify";

        public const string ROLE_LECTOR = "Lector";
        public const string ROLE_EDITOR = "Editor";
        public const string ROLE_ADMINISTRADOR = "Administrador";

        public const string CACHE_NEWS = "news";
        public const string CACHE_USERS = "users";
        public const string CACHE_IMAGES = "images";
        public const string CACHE_CHARGES = "charges";
        public const string CACHE_ABOUT_US = "about_us";
        public const string CACHE_STATUSES = "statuses";
        public const string CACHE_CONTACTS = "contacts";
        public const string CACHE_DOCUMENTS = "documents";
        public const string CACHE_PRINCIPALS = "principals";
        public const string CACHE_CATEGORIES = "categories";
        public const string CACHE_CONFIGURATIONS = "configurations";

        public static readonly string[] ALLOWED_HTTP_METHODS = new string[] { "GET", "POST", "PUT", "DELETE", "PATCH" };
        public static readonly string[] ALLOWED_HTTP_HEADERS = new string[] { "Content-Type", "Authorization", "x-version", "Accept", "Origin", "X-Requested-With" };
    }
}
