namespace Hydra.Infrastructure.Test.I18n
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class CultureHelperTests
    {
        [Fact]
        public void Should_Parse_User_Cultures_From_Accept_Language_String()
        {
            CultureInfo[] actual = CultureHelper.ParseUserCultures("fr-FR , en;q=0.8 , en-us;q=0.5 , de;q=0.3");

            var expected = new[]
            {
                Cultures.FrenchFrance, 
                Cultures.EnglishNeutral, 
                Cultures.EnglishUnitedStates, 
                Cultures.GermanNeutral
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Get_User_Culture_From_Header_And_Supported_Languages()
        {
            NameValueCollection headers = new NameValueCollection();

            headers[HttpHeaders.AcceptLanguage] = "fr-FR , en;q=0.8 , en-us;q=0.5 , de;q=0.3";

            CultureInfo actual = CultureHelper.GetUserCulture(headers);

            CultureInfo expected = Cultures.EnglishUnitedStates;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Set_Preferred_Culture_To_Cookie()
        {
            DateTime now = DateTime.Now.Freeze();

            HttpCookieCollection cookies = new HttpCookieCollection();

            CultureHelper.SetPreferredCulture(cookies, Cultures.GermanGermany);

            HttpCookie actual = cookies.Get(0);

            Assert.NotNull(actual);
            Assert.Equal(now.Add(30.Days()), actual.Expires);
            Assert.Equal("PreferredCulture", actual.Name);
            Assert.Equal("de-DE", actual.Value);
        }

        [Fact]
        public void Should_Get_Preferred_Culture_From_Cookies()
        {
            HttpCookieCollection cookies = new HttpCookieCollection();

            CultureHelper.SetPreferredCulture(cookies, Cultures.GermanGermany);

            CultureInfo actual = CultureHelper.GetPreferredCulture(cookies);

            CultureInfo expected = Cultures.GermanGermany;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Apply_Preferred_Culture_From_Header()
        {
            SupportedCulturesProvider.Current = new InMemorySupportedCulturesProvider(Cultures.EnglishUnitedStates, Cultures.GermanGermany, Cultures.FrenchFrance);

            NameValueCollection headers = new NameValueCollection { { HttpHeaders.AcceptLanguage, "fr-FR" } };

            HttpCookieCollection cookies = new HttpCookieCollection();

            CultureHelper.ApplyUserCulture(headers, cookies);

            Assert.Equal(Cultures.FrenchFrance, CultureInfo.CurrentCulture);
            Assert.Equal(Cultures.FrenchFrance, CultureInfo.CurrentUICulture);
        }

        [Fact]
        public void Should_Apply_Preferred_Culture_From_Cookie()
        {
            SupportedCulturesProvider.Current = new InMemorySupportedCulturesProvider(Cultures.EnglishUnitedStates, Cultures.GermanGermany, Cultures.FrenchFrance);

            NameValueCollection headers = new NameValueCollection();

            HttpCookieCollection cookies = new HttpCookieCollection();

            CultureHelper.SetPreferredCulture(cookies, Cultures.FrenchFrance);

            CultureHelper.ApplyUserCulture(headers, cookies);

            Assert.Equal(Cultures.FrenchFrance, CultureInfo.CurrentCulture);
            Assert.Equal(Cultures.FrenchFrance, CultureInfo.CurrentUICulture);
        }
    }
}
