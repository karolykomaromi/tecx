namespace Hydra.Infrastructure.Test.I18n
{
    using System.Collections.Generic;
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class PolyglotStringTests
    {
        [Fact]
        public void Should_Deserialize_From_Json()
        {
            string s = "{\"de-DE\":\"de\",\"en-US\":\"us\"}";

            PolyglotString mls;

            Assert.True(PolyglotString.TryParse(s, out mls));
        }

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

            PolyglotString mls1 = new PolyglotString(translations1);

            PolyglotString mls2 = new PolyglotString(translations2);

            Assert.Equal(mls1, mls2);
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
            PolyglotString mls = new PolyglotString(Cultures.GermanGermany, "specific");

            Assert.Equal("specific", mls.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Find_Neutral_Translation_As_Fallback()
        {
            PolyglotString mls = new PolyglotString(Cultures.GermanNeutral, "neutral");

            Assert.Equal("neutral", mls.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Find_Invariant_Translation_As_Fallback()
        {
            PolyglotString mls = new PolyglotString(CultureInfo.InvariantCulture, "invariant");

            Assert.Equal("invariant", mls.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Use_Single_Existing_Translation_As_Fallback()
        {
            PolyglotString mls = new PolyglotString(Cultures.EnglishUnitedStates, "single");

            Assert.Equal("single", mls.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Create_PolyglotString_From_Resource()
        {
            PolyglotString actual = PolyglotString.FromResource(() => Properties.Resources.FromResource);

            Assert.NotNull(actual);
            Assert.NotSame(PolyglotString.Empty, actual);
            Assert.Equal("en-US", actual.Translations[Cultures.EnglishUnitedStates]);
            Assert.Equal("de-DE", actual.Translations[Cultures.GermanGermany]);
        }
    }
}
