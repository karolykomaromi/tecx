namespace Cars.I18n
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Cars.Extensions;
    using Newtonsoft.Json;

    public static class PolyglotStringExtensions
    {
        public static string ToJson(this PolyglotString ps)
        {
            Contract.Requires(ps != null);
            Contract.Ensures(Contract.Result<string>() != null);

            string s = JsonConvert.SerializeObject(ps.Translations, Formatting.None);

            return s ?? string.Empty;
        }

        public static PolyglotString FromJson(string s)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(s));
            Contract.Ensures(Contract.Result<PolyglotString>() != null);

            Dictionary<CultureInfo, string> translations = JsonConvert.DeserializeObject<Dictionary<CultureInfo, string>>(s);

            return new PolyglotString(translations);
        }

        public static XElement ToXml(this PolyglotString ps)
        {
            Contract.Requires(ps != null);
            Contract.Ensures(Contract.Result<XElement>() != null);

            XElement xml = new XElement("ps");

            foreach (var translation in ps.Translations)
            {
                XElement t = new XElement("t", new XAttribute("l", translation.Key.Name), new XAttribute("v", translation.Value));

                xml.Add(t);
            }

            return xml;
        }

        public static PolyglotString FromXml(XElement xml)
        {
            PolyglotStringBuilder builder = new PolyglotStringBuilder();

            foreach (XElement translation in xml.Descendants("t"))
            {
                XAttribute l = translation.Attribute("l");

                if (l != null && !string.IsNullOrEmpty(l.Value))
                {
                    var language = CultureInfo.GetCultureInfo(l.Value);

                    XAttribute v = translation.Attribute("v");

                    if (v != null && !string.IsNullOrEmpty(v.Value))
                    {
                        builder.WithTranslation(language, v.Value);
                    }
                }
            }

            return builder;
        }

        public static PolyglotString Append(this string s, PolyglotString ps)
        {
            Contract.Requires(s != null);
            Contract.Requires(ps != null);
            Contract.Ensures(Contract.Result<PolyglotString>() != null);

            return new PolyglotString(s.Append(ps.Translations));
        }

        public static PolyglotString Append(this PolyglotString ps, string s)
        {
            Contract.Requires(ps != null);
            Contract.Requires(s != null);
            Contract.Ensures(Contract.Result<PolyglotString>() != null);

            return new PolyglotString(ps.Translations.Append(s));
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