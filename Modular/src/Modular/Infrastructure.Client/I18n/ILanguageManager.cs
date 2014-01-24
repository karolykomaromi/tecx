namespace Infrastructure.I18n
{
    using System;

    public interface ILanguageManager
    {
        event EventHandler LanguageChanged;

        void NotifyApplicationLanguageChanged();
    }
}
