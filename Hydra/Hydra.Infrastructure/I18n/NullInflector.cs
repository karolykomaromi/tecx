namespace Hydra.Infrastructure.I18n
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    public class NullInflector : IInflector
    {
        public IReadOnlyCollection<CultureInfo> SupportedCultures
        {
            get
            {
                return new ReadOnlyCollection<CultureInfo>(new CultureInfo[0]);
            }
        }

        public string Pluralize(string word)
        {
            return word;
        }

        public string Singularize(string word)
        {
            return word;
        }
    }
}