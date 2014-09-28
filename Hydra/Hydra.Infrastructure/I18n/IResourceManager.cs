namespace Hydra.Infrastructure.I18n
{
    using System.Globalization;

    public interface IResourceManager
    {
        string GetString(string name, CultureInfo culture);
    }
}
