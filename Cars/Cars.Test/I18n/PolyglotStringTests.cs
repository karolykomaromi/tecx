namespace Cars.Test.I18n
{
    using System.Collections.Generic;
    using System.Globalization;
    using Cars.I18n;
    using Xunit;

    public class PolyglotStringTests
    {
        [Fact]
        public void Should_Recognize_Equal_Strings()
        {
            var translations1 = new Dictionary<CultureInfo, string>
                               {
                                   { Cultures.GermanGermany, "de" },
                                   { Cultures.EnglishUnitedStates, "us" }
                               };

            var translations2 = new Dictionary<CultureInfo, string>
                               {
                                   { Cultures.EnglishUnitedStates, "us" },
                                   { Cultures.GermanGermany, "de" }
                               };

            PolyglotString x = new PolyglotString(translations1);

            PolyglotString y = new PolyglotString(translations2);

            Assert.Equal(x, y);
        }
        
        [Fact]
        public void Clone_Should_Create_New_Instance()
        {
            PolyglotString original = new PolyglotString(Cultures.GermanGermany, "de");

            PolyglotString clone = original.Clone();

            Assert.NotSame(original, clone);
        }

        [Fact]
        public void Cloned_String_Should_Equal_Original()
        {
            PolyglotString original = new PolyglotString(Cultures.GermanGermany, "de");

            PolyglotString clone = original.Clone();

            Assert.Equal(original, clone);
        }

        [Fact]
        public void Should_Find_Specific_Translation()
        {
            PolyglotString ps = new PolyglotString(Cultures.GermanGermany, "specific");

            Assert.Equal("specific", ps.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Find_Neutral_Translation_As_Fallback()
        {
            PolyglotString ps = new PolyglotString(Cultures.GermanNeutral, "neutral");

            Assert.Equal("neutral", ps.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Find_Invariant_Translation_As_Fallback()
        {
            PolyglotString ps = new PolyglotString(CultureInfo.InvariantCulture, "invariant");

            Assert.Equal("invariant", ps.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Use_Single_Existing_Translation_As_Fallback()
        {
            PolyglotString ps = new PolyglotString(Cultures.EnglishUnitedStates, "single");

            Assert.Equal("single", ps.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Create_PolyglotString_From_Resource()
        {
            PolyglotString actual = PolyglotString.FromResource(() => Properties.Resources.FromResources);

            Assert.NotNull(actual);
            Assert.NotSame(PolyglotString.Empty, actual);
            Assert.Equal("en-US", actual.Translations[Cultures.EnglishUnitedStates]);
            Assert.Equal("de-DE", actual.Translations[Cultures.GermanGermany]);
        }

        [Fact]
        public void Should_Format_PolyglotString_With_Correct_Culture_For_Each_Translation()
        {
            PolyglotString ps = new PolyglotStringBuilder()
                .GermanGermany("de-DE: {0:F3}")
                .EnglishUnitedStates("en-US: {0:F3}");

            PolyglotString actual = PolyglotString.Format(ps, 1.234);

            Assert.Equal("de-DE: 1,234", actual.ToString(Cultures.GermanGermany));
            Assert.Equal("en-US: 1.234", actual.ToString(Cultures.EnglishUnitedStates));
        }

        [Fact]
        public void Should_Format_PolyglotString_With_Polystring()
        {
            PolyglotString ps1 = new PolyglotStringBuilder()
                .GermanGermany("de-DE:{0}")
                .EnglishUnitedStates("en-US:{0}");

            PolyglotString ps2 = new PolyglotStringBuilder()
                .GermanGermany("de-DE")
                .EnglishUnitedStates("en-US");

            PolyglotString actual = PolyglotString.Format(ps1, ps2);

            Assert.Equal("de-DE:de-DE", actual.ToString(Cultures.GermanGermany));
            Assert.Equal("en-US:en-US", actual.ToString(Cultures.EnglishUnitedStates));
        }
    }
}
