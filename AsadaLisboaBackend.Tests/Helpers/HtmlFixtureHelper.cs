namespace AsadaLisboaBackend.Tests.Receipts.Helpers
{
    public static class HtmlFixtureHelper
    {
        public static string Load(string fileName)
        {
            var path = Path.Combine(
                AppContext.BaseDirectory,
                "Receipts",
                "Fixtures",
                fileName);

            return File.ReadAllText(path);
        }
    }
}