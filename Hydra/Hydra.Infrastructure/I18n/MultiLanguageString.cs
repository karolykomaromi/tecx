namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using Hydra.Infrastructure.Logging;
    using Newtonsoft.Json;

    public class MultiLanguageString : IEquatable<MultiLanguageString>, IComparable<MultiLanguageString>, IFreezable<MultiLanguageString>, ICloneable<MultiLanguageString>
    {
        public static readonly MultiLanguageString Empty = new MultiLanguageString().Freeze();

        private readonly IDictionary<CultureInfo, string> translations;

        private MultiLanguageString()
        {
            this.translations = new SortedDictionary<CultureInfo, string>(new CultureComparer());
        }

        public virtual bool IsMutable
        {
            get { return true; }
        }

        public static MultiLanguageString New(CultureInfo culture, string translation)
        {
            Contract.Requires(culture != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(translation));
            Contract.Ensures(Contract.Result<MultiLanguageString>() != null);

            var mls = new MultiLanguageString().WithTranslation(culture, translation);

            return mls;
        }

        public static bool TryParse(string s, out MultiLanguageString mls)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(s));

            try
            {
                Dictionary<CultureInfo, string> t = JsonConvert.DeserializeObject<Dictionary<CultureInfo, string>>(s);

                mls = new MultiLanguageString();

                foreach (var translation in t)
                {
                    mls.translations[translation.Key] = translation.Value;
                }

                return true;
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                mls = MultiLanguageString.Empty;
                return false;
            }
        }

        public MultiLanguageString Freeze()
        {
            return new ImmutableMultiLanguageString(this);
        }

        public virtual MultiLanguageString WithTranslation(CultureInfo culture, string translation)
        {
            Contract.Requires(culture != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(translation));

            this.translations[culture] = translation;

            return this;
        }

        public int CompareTo(MultiLanguageString other)
        {
            if (other == null)
            {
                return 1;
            }

            int compare = string.Compare(this.ToString(), other.ToString(), StringComparison.Ordinal);

            return compare;
        }

        public bool Equals(MultiLanguageString other)
        {
            if (other == null)
            {
                return false;
            }

            bool @equals = string.Equals(this.ToString(), other.ToString(), StringComparison.Ordinal);

            return @equals;
        }

        public override bool Equals(object obj)
        {
            MultiLanguageString other = obj as MultiLanguageString;

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

        public virtual MultiLanguageString Clone()
        {
            MultiLanguageString clone = new MultiLanguageString();

            foreach (var translation in this.translations)
            {
                clone.translations[translation.Key] = translation.Value;
            }

            return clone;
        }

        public override string ToString()
        {
            string s = JsonConvert.SerializeObject(this.translations, Formatting.None);

            return s;
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

                int compare = string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal);

                return compare;
            }
        }

        private class ImmutableMultiLanguageString : MultiLanguageString
        {
            public ImmutableMultiLanguageString(MultiLanguageString mls)
            {
                Contract.Requires(mls != null);

                foreach (var translation in mls.translations)
                {
                    this.translations[translation.Key] = translation.Value;
                }
            }

            public override bool IsMutable
            {
                get { return false; }
            }

            public override MultiLanguageString Clone()
            {
                return base.Clone().Freeze();
            }

            public override MultiLanguageString WithTranslation(CultureInfo culture, string translation)
            {
                return this;
            }
        }
    }
}