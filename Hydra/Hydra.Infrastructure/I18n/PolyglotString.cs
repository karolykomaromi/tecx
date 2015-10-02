namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;
    using Newtonsoft.Json;

    public class PolyglotString : IEquatable<PolyglotString>, IComparable<PolyglotString>, ICloneable<PolyglotString>
    {
        public static readonly PolyglotString Empty = new PolyglotString();

        private readonly IDictionary<CultureInfo, string> translations;

        public PolyglotString(CultureInfo culture, string translation)
        {
            Contract.Requires(culture != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(translation));

            this.translations = new SortedDictionary<CultureInfo, string>(new CultureComparer())
                                {
                                    { culture, translation }
                                };
        }

        public PolyglotString(IEnumerable<KeyValuePair<CultureInfo, string>> translations)
        {
            Contract.Requires(translations != null);

            this.translations = new SortedDictionary<CultureInfo, string>(
                translations.ToDictionary(x => x.Key, x => x.Value ?? string.Empty),
                new CultureComparer());
        }

        private PolyglotString()
        {
            this.translations = new Dictionary<CultureInfo, string>();
        }

        public IDictionary<CultureInfo, string> Translations
        {
            get { return new Dictionary<CultureInfo, string>(this.translations); }
        }

        public static PolyglotString FromResource(Expression<Func<string>> resourceSelector)
        {
            Contract.Requires(resourceSelector != null);
            Contract.Ensures(Contract.Result<PolyglotString>() != null);

            MemberExpression memberExpression = resourceSelector.Body as MemberExpression;

            if (memberExpression != null)
            {
                MemberInfo member = memberExpression.Member;

                if (member.DeclaringType != null)
                {
                    PropertyInfo rm = member
                        .DeclaringType
                        .GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public);

                    if (rm != null)
                    {
                        IResourceManager resourceManager = rm.GetValue(null, null) as IResourceManager;

                        if (resourceManager != null)
                        {
                            var translations = SupportedCulturesProvider.Current.GetSupportedCultures()
                                .Select(
                                    culture =>
                                        new
                                        {
                                            Culture = culture,
                                            Translation = resourceManager.GetString(member.Name, culture)
                                        })
                                .ToDictionary(x => x.Culture, x => x.Translation);

                            return new PolyglotString(translations);
                        }
                    }
                }
            }

            return PolyglotString.Empty;
        }

        public static bool TryParse(string s, out PolyglotString mls)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(s));

            try
            {
                Dictionary<CultureInfo, string> t = JsonConvert.DeserializeObject<Dictionary<CultureInfo, string>>(s);

                mls = new PolyglotString(t);

                return true;
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                mls = PolyglotString.Empty;

                return false;
            }
        }
        
        public int CompareTo(PolyglotString other)
        {
            if (other == null)
            {
                return 1;
            }

            // weberse 2015-09-14 We sort using the current UI culture
            int compare = string.Compare(this.ToString(), other.ToString(), false, CultureInfo.CurrentUICulture);

            return compare;
        }

        public bool Equals(PolyglotString other)
        {
            if (other == null)
            {
                return false;
            }

            foreach (CultureInfo culture in this.translations.Keys.Union(other.translations.Keys))
            {
                // weberse 2015-09-14 If any key is only in one dictionary but not the other the strings are not equal
                string x, y;
                if (!TranslationExistsInBothStrings(this.translations, other.translations, culture, out x, out y))
                {
                    return false;
                }

                // weberse 2015-09-14 If the strings for a given language are not equal when compared in that language
                // the strings are not equal
                if (string.Compare(x, y, false, culture) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            PolyglotString other = obj as PolyglotString;

            if (other == null)
            {
                return false;
            }

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public virtual PolyglotString Clone()
        {
            PolyglotString clone = new PolyglotString(this.translations);

            return clone;
        }

        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }

        public virtual string ToString(CultureInfo culture)
        {
            Contract.Requires(culture != null);

            // specific culture e.g. en-US
            string translation;
            if (this.translations.TryGetValue(culture, out translation))
            {
                return translation;
            }

            // neutral culture e.g. en
            if (this.translations.TryGetValue(culture.Parent, out translation))
            {
                return translation;
            }

            // invariant culture
            if (this.translations.TryGetValue(culture.Parent.Parent, out translation))
            {
                return translation;
            }

            if (this.translations.Count == 1)
            {
                translation = this.translations.First().Value;

                return translation;
            }

            return Properties.Resources.TranslationNotFound;
        }
        
        private static bool TranslationExistsInBothStrings(
            IDictionary<CultureInfo, string> first,
            IDictionary<CultureInfo, string> second,
            CultureInfo culture,
            out string x,
            out string y)
        {
            if (!first.TryGetValue(culture, out x) || !second.TryGetValue(culture, out y))
            {
                x = string.Empty;
                y = string.Empty;

                return false;
            }

            return true;
        }

        private class CultureComparer : IComparer<CultureInfo>
        {
            public int Compare(CultureInfo x, CultureInfo y)
            {
                // From MSDN: By definition, any object compares greater than null, and two null references compare equal to each other.
                // https://msdn.microsoft.com/en-us/library/43hc6wht%28v=vs.110%29.aspx
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                int compare = string.Compare(x.Name, y.Name, StringComparison.Ordinal);

                return compare;
            }
        }
    }
}