namespace Infrastructure.I18n
{
    using System.Globalization;

    public class EchoResourceManager : IResourceManager
    {
        public string GetString(string name, CultureInfo culture)
        {
            return name;
        }
    }
}