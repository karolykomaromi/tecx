namespace Infrastructure.I18n
{
    using System.Globalization;

    public class WebSvcResourceManager : IResourceManager
    {
        public string this[ResxKey key]
        {
            get { return key.ToString(); }
        }

        public string GetString(string name, CultureInfo culture)
        {
            return name.ToUpperInvariant();
        }
    }
}