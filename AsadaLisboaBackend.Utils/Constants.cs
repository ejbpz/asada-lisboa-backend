using System.Text.RegularExpressions;

namespace AsadaLisboaBackend.Utils
{
    public static class Constants
    {
        public static string RESEND_API_TOKEN = "RESEND_API_TOKEN";
        public static string DOMAIN_HOST = "https://asadalisboa.or.cr";
        public static Regex EMAIL_REGEX = new Regex("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$");
    }
}
