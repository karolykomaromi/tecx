using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Cars.I18n
{
    public class PolyglotStringBuilder : Builder<PolyglotString>
    {
        private readonly IDictionary<CultureInfo, string> translations;

        public PolyglotStringBuilder()
        {
            this.translations = new Dictionary<CultureInfo, string>();
        }

        public PolyglotStringBuilder WithTranslation(CultureInfo culture, string translation)
        {
            Contract.Requires(culture != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(translation));
            Contract.Ensures(Contract.Result<PolyglotStringBuilder>() != null);

            this.translations[culture] = translation;

            return this;
        }

        public override PolyglotString Build()
        {
            if (this.translations.Count == 0)
            {
                return PolyglotString.Empty;
            }

            return new PolyglotString(this.translations);
        }
    }
}