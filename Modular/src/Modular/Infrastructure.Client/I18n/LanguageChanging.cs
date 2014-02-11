namespace Infrastructure.I18n
{
    using System.Globalization;

    public class LanguageChanging
    {
        private readonly CultureInfo culture;

        public LanguageChanging(CultureInfo culture)
        {
            this.culture = culture;
        }

        public CultureInfo Culture
        {
            get { return this.culture; }
        }
    }
}