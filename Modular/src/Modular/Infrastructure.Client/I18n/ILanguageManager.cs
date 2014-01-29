namespace Infrastructure.I18n
{
    using System;
    using System.Globalization;

    public interface ILanguageManager
    {
        event EventHandler LanguageChanged;

        CultureInfo CurrentCulture { get; }

        void ChangeLanguage(CultureInfo newCulture);
    }
}
