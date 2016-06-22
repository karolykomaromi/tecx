namespace Cars.Test.I18n
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;
    using Cars.I18n;
    using Xunit;

    public class PolyglotStringExtensionsTests
    {
        [Fact]
        public void Should_Concat_Strings_With_Matching_Keys()
        {
            PolyglotString x = new PolyglotString(Cultures.GermanNeutral, "1");

            PolyglotString y = new PolyglotString(Cultures.GermanNeutral, "2");

            PolyglotString actual = x.Append(y);

            Assert.Equal("12", actual.ToString(Cultures.GermanNeutral));
        }

        [Fact]
        public void Should_Outer_Join_Strings_With_Non_Matching_Keys()
        {
            PolyglotString x = new PolyglotString(Cultures.GermanNeutral, "1");

            PolyglotString y = new PolyglotString(Cultures.EnglishNeutral, "2");

            PolyglotString actual = x.Append(y);

            Assert.Equal("1", actual.ToString(Cultures.GermanNeutral));
            Assert.Equal("2", actual.ToString(Cultures.EnglishNeutral));
        }

        [Fact]
        public void Should_Append_String_To_All_Entries()
        {
            PolyglotString ps = new PolyglotString(new Dictionary<CultureInfo, string> { { Cultures.GermanNeutral, "1" }, { Cultures.EnglishNeutral, "2" } });

            PolyglotString actual = ps.Append(" ");

            Assert.Equal("1 ", actual.ToString(Cultures.GermanNeutral));
            Assert.Equal("2 ", actual.ToString(Cultures.EnglishNeutral));
        }

        [Fact]
        public void Should_Prepend_String_To_All_Entries()
        {
            PolyglotString ps = new PolyglotString(new Dictionary<CultureInfo, string> { { Cultures.GermanNeutral, "1" }, { Cultures.EnglishNeutral, "2" } });

            PolyglotString actual = " ".Append(ps);

            Assert.Equal(" 1", actual.ToString(Cultures.GermanNeutral));
            Assert.Equal(" 2", actual.ToString(Cultures.EnglishNeutral));
        }

        [Fact]
        public void Should_Convert_String_To_Xml()
        {
            XElement expected = new XElement(
                "ps", 
                new XElement("t", new XAttribute("l", "de-DE"), new XAttribute("v", "de-DE")), 
                new XElement("t", new XAttribute("l", "en-US"), new XAttribute("v", "en-US")));

            PolyglotString ps = new PolyglotStringBuilder().GermanGermany("de-DE").EnglishUnitedStates("en-US");

            XElement actual = ps.ToXml();

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void Should_Convert_Xml_To_String()
        {
            XElement xml = new XElement(
                "ps",
                new XElement("t", new XAttribute("l", "de-DE"), new XAttribute("v", "de-DE")),
                new XElement("t", new XAttribute("l", "en-US"), new XAttribute("v", "en-US")));

            PolyglotString actual = PolyglotStringExtensions.FromXml(xml);

            PolyglotString expected = new PolyglotStringBuilder().GermanGermany("de-DE").EnglishUnitedStates("en-US");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Convert_String_To_Json()
        {
            PolyglotString ps = new PolyglotStringBuilder().GermanGermany("de-DE").EnglishUnitedStates("en-US");

            string actual = ps.ToJson();

            string expected = "{\"de-DE\":\"de-DE\",\"en-US\":\"en-US\"}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Convert_Json_To_String()
        {
            string json = "{\"de-DE\":\"de-DE\",\"en-US\":\"en-US\"}";

            PolyglotString actual = PolyglotStringExtensions.FromJson(json);

            PolyglotString expected = new PolyglotStringBuilder().GermanGermany("de-DE").EnglishUnitedStates("en-US");

            Assert.Equal(expected, actual);
        }
    }
}