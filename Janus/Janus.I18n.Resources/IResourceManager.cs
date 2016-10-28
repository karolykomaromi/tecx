namespace Janus.I18n.Resources
{
    using System.Globalization;

    public interface IResourceManager
    {
        string GetString(string name, CultureInfo culture);

        object GetObject(string name, CultureInfo culture);
    }
}