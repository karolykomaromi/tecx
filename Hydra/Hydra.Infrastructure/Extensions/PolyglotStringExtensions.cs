namespace Hydra.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using Hydra.Infrastructure.I18n;

    public static class PolyglotStringExtensions
    {
        public static PolyglotString Append(this string s, PolyglotString mls)
        {
            Contract.Requires(s != null);
            Contract.Requires(mls != null);
            Contract.Ensures(Contract.Result<PolyglotString>() != null);

            return new PolyglotString(s.Append(mls.Translations));
        }

        public static PolyglotString Append(this PolyglotString mls, string s)
        {
            Contract.Requires(mls != null);
            Contract.Requires(s != null);
            Contract.Ensures(Contract.Result<PolyglotString>() != null);

            return new PolyglotString(mls.Translations.Append(s));
        }

        public static PolyglotString Append(this PolyglotString first, PolyglotString second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Ensures(Contract.Result<PolyglotString>() != null);

            return new PolyglotString(first.Translations.Append(second.Translations));
        }

        private static IEnumerable<KeyValuePair<CultureInfo, string>> Append(
            this string s,
            IEnumerable<KeyValuePair<CultureInfo, string>> translations)
        {
            Contract.Requires(s != null);
            Contract.Requires(translations != null);
            Contract.Ensures(Contract.Result<IEnumerable<KeyValuePair<CultureInfo, string>>>() != null);

            return translations.ToDictionary(x => x.Key, x => s + (x.Value ?? string.Empty));
        }

        private static IEnumerable<KeyValuePair<CultureInfo, string>> Append(
            this IEnumerable<KeyValuePair<CultureInfo, string>> translations,
            string s)
        {
            Contract.Requires(translations != null);
            Contract.Requires(s != null);
            Contract.Ensures(Contract.Result<IEnumerable<KeyValuePair<CultureInfo, string>>>() != null);

            return translations.ToDictionary(x => x.Key, x => (x.Value ?? string.Empty) + s);
        }

        private static IEnumerable<KeyValuePair<CultureInfo, string>> Append(
            this IDictionary<CultureInfo, string> first,
            IDictionary<CultureInfo, string> second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Ensures(Contract.Result<IEnumerable<KeyValuePair<CultureInfo, string>>>() != null);

            return first.Merge(second, g => string.Join(string.Empty, g.Select(s => s ?? string.Empty)));
        }
    }
}