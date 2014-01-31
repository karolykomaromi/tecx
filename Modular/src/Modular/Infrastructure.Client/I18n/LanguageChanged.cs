namespace Infrastructure.I18n
{
    using System.Globalization;

    public class LanguageChanged
    {
        private readonly CultureInfo culture;

        public LanguageChanged(CultureInfo culture)
        {
            this.culture = culture;
        }

        public CultureInfo Culture
        {
            get { return culture; }
        }
    }
}