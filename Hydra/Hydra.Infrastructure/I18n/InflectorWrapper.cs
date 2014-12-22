namespace Hydra.Infrastructure.I18n
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    public class InflectorWrapper : IInflector
    {
        private readonly CultureInfo culture;

        public InflectorWrapper()
        {
            this.culture = Cultures.EnglishUnitedStates;
        }

        public IReadOnlyCollection<CultureInfo> SupportedCultures
        {
            get
            {
                return new ReadOnlyCollection<CultureInfo>(new[] { this.culture });
            }
        }

        public string Pluralize(string word)
        {
            return Inflector.Pluralize(word);
        }

        public string Singularize(string word)
        {
            return Inflector.Singularize(word);
        }
    }
}