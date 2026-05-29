using AngleSharp.Dom;

namespace AsadaLisboaBackend.Utils.HtmlSanitizer
{
    public static class HtmlSanitizerFactory
    {
        public static Ganss.Xss.HtmlSanitizer Create()
        {
            var sanitizer = new Ganss.Xss.HtmlSanitizer();

            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedTags.UnionWith(new[]
            {
                "p", "span",
                "h2", "h3", "h4", "h5", "h6",
                "strong", "b", "em", "i", "u",
                "ul", "ol", "li",
                "br",
                "img",
                "a"
            });

            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedAttributes.UnionWith(new[]
            {
                "class",
                "href",
                "target",
                "rel",
                "src",
                "alt",
                "title"
            });

            sanitizer.AllowedSchemes.Clear();
            sanitizer.AllowedSchemes.UnionWith(new[] { "http", "https" });
            sanitizer.AllowedCssProperties.Clear();

            sanitizer.RemovingAttribute += (sender, eventArgs) =>
            {
                if (eventArgs.Attribute != null && eventArgs.Attribute.Name.StartsWith("on"))
                    eventArgs.Cancel = true;
            };

            sanitizer.PostProcessNode += (sender, eventArgs) =>
            {
                if(eventArgs.Node is not IElement element)
                    return;

                if (element.TagName.Equals("a", StringComparison.OrdinalIgnoreCase))
                {
                    var href = element.GetAttribute("href");

                    if (!string.IsNullOrWhiteSpace(href))
                    {
                        if (!Uri.TryCreate(href, UriKind.Absolute, out var uri) ||
                            (uri.Scheme != "http" && uri.Scheme != "https"))
                        {
                            element.RemoveAttribute("href");
                        }
                    }

                    element.SetAttribute("rel", "noopener noreferrer");
                    element.SetAttribute("target", "_blank");
                }

                if (element.TagName.Equals("img", StringComparison.OrdinalIgnoreCase))
                {
                    var src = element.GetAttribute("src");

                    if (string.IsNullOrWhiteSpace(src))
                    {
                        element.Remove();
                        return;
                    }
                }
            };

            return sanitizer;
        }
    }
}
