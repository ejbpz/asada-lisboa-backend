using System.Text.RegularExpressions;

namespace AsadaLisboaBackend.Utils.SlugGeneration
{
    public static class GenerateSlug
    {
        public static string New(string text, Guid id)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("El texto para el slug es inválido");

            var slug = text.ToLower().Trim();

            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = Regex.Replace(slug, @"-+", "-");
            slug = slug.Trim('-'); 

            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            var shortId = id.ToString("N")[..6];

            return $"{slug}-{timestamp}-{shortId}";
        }
    }
}
