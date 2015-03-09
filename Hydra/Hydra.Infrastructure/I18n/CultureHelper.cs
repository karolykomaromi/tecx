namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using Hydra.Infrastructure.Logging;

    public static class CultureHelper
    {
        public static CultureInfo[] ParseUserCultures(string acceptLanguage)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(acceptLanguage));
            Contract.Ensures(Contract.Result<CultureInfo[]>() != null);

            // Accept-Language: fr-FR , en;q=0.8 , en-us;q=0.5 , de;q=0.3
            if (string.IsNullOrWhiteSpace(acceptLanguage))
            {
                return new CultureInfo[0];
            }

            CultureInfo[] cultures = acceptLanguage.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(WeightedLanguage.Parse)
                .OrderByDescending(w => w.Weight)
                .Select(w => GetCultureInfo(w.Language))
                .Where(culture => culture != null)
                .ToArray();

            return cultures;
        }

        public static CultureInfo GetUserCulture(NameValueCollection headers)
        {
            Contract.Requires(headers != null);

            CultureInfo[] acceptedCultures = ParseUserCultures(headers[HttpHeaders.AcceptLanguage]);

            CultureInfo culture = GetMatchingCulture(acceptedCultures, SupportedCulturesProvider.SupportedCultures);

            return culture;
        }

        public static void ApplyUserCulture(NameValueCollection headers, HttpCookieCollection cookies)
        {
            Contract.Requires(cookies != null);

            CultureInfo culture = GetPreferredCulture(cookies)
                ?? GetUserCulture(headers)
                ?? SupportedCulturesProvider.SupportedCultures[0];

            Thread t = Thread.CurrentThread;
            t.CurrentCulture = culture;
            t.CurrentUICulture = culture;

            HydraEventSource.Log.CultureChanged(culture);
        }

        public static CultureInfo GetPreferredCulture(HttpCookieCollection cookies)
        {
            Contract.Requires(cookies != null);

            HttpCookie cookie = cookies[Constants.Cookies.PreferredCulture];

            if (cookie == null)
            {
                return null;
            }

            var culture = GetCultureInfo(cookie.Value);

            if (culture == null)
            {
                return null;
            }

            if (SupportedCulturesProvider.SupportedCultures.All(ci => !string.Equals(ci.Name, culture.Name, StringComparison.Ordinal)))
            {
                return null;
            }

            return culture;
        }

        public static void SetPreferredCulture(HttpCookieCollection cookies, CultureInfo culture)
        {
            Contract.Requires(cookies != null);
            Contract.Requires(culture != null);

            HttpCookie cookie = new HttpCookie(Constants.Cookies.PreferredCulture, culture.ToString())
                {
                    Expires = TimeProvider.Now.AddDays(30)
                };

            cookies.Set(cookie);
        }
        
        private static CultureInfo GetMatchingCulture(CultureInfo[] acceptedCultures, IReadOnlyCollection<CultureInfo> supportedCultures)
        {
            Contract.Requires(acceptedCultures != null);
            Contract.Requires(supportedCultures != null);

            return

                // first pass: exact matches as well as requested neutral matching supported region
                // supported: en-US, de-DE
                // requested: de, en-US;q=0.8
                // => de-DE! (de has precendence over en-US)
                GetMatch(acceptedCultures, supportedCultures, MatchesCompletely)

                    // second pass: look for requested neutral matching supported _neutral_ region
                    // supported: en-US, de-DE
                    // requested: de-AT, en-GB;q=0.8
                    // => de-DE! (no exact match, but de-AT has better fit than en-GB)
                ?? GetMatch(acceptedCultures, supportedCultures, MatchesPartly);
        }

        private static CultureInfo GetMatch(CultureInfo[] acceptedCultures, IReadOnlyCollection<CultureInfo> supportedCultures, Func<CultureInfo, CultureInfo, bool> predicate)
        {
            foreach (var acceptedCulture in acceptedCultures)
            {
                CultureInfo match = supportedCultures.FirstOrDefault(supportedCulture => predicate(acceptedCulture, supportedCulture));

                if (match != null)
                {
                    return match;
                }
            }

            return null;
        }

        private static bool MatchesCompletely(CultureInfo acceptedCulture, CultureInfo supportedCulture)
        {
            if (string.Equals(supportedCulture.Name, acceptedCulture.Name, StringComparison.Ordinal))
            {
                return true;
            }

            // acceptedCulture could be neutral and supportedCulture specific, but this is still a match (de matches de-DE, de-AT, ...)
            if (acceptedCulture.IsNeutralCulture &&
                string.Equals(supportedCulture.Parent.Name, acceptedCulture.Name, StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }

        private static bool MatchesPartly(CultureInfo acceptedCulture, CultureInfo supportedCulture)
        {
            supportedCulture = supportedCulture.Parent;

            if (!acceptedCulture.IsNeutralCulture)
            {
                acceptedCulture = acceptedCulture.Parent;
            }

            if (string.Equals(supportedCulture.Name, acceptedCulture.Name, StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }

        private static CultureInfo GetCultureInfo(string language)
        {
            try
            {
                return CultureInfo.GetCultureInfo(language);
            }
            catch (CultureNotFoundException)
            {
                return null;
            }
        }
    }
}