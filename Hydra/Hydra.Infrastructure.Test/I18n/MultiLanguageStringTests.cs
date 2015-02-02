namespace Hydra.Infrastructure.Test.I18n
{
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class MultiLanguageStringTests
    {
        [Fact]
        public void Should_Serialize_To_Json()
        {
            MultiLanguageString mls = MultiLanguageString.New(Cultures.GermanGermany, "de").WithTranslation(Cultures.EnglishUnitedStates, "us");

            string actual = mls.ToString();

            string expected = "{\"de-DE\":\"de\",\"en-US\":\"us\"}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Deserialize_From_Json()
        {
            string s = "{\"de-DE\":\"de\",\"en-US\":\"us\"}";

            MultiLanguageString mls;

            Assert.True(MultiLanguageString.TryParse(s, out mls));
        }

        [Fact]
        public void Should_Recognize_Equal_Strings()
        {
            MultiLanguageString mls1 = MultiLanguageString.New(Cultures.GermanGermany, "de").WithTranslation(Cultures.EnglishUnitedStates, "us");

            MultiLanguageString mls2 = MultiLanguageString.New(Cultures.EnglishUnitedStates, "us").WithTranslation(Cultures.GermanGermany, "de");

            Assert.Equal(mls1, mls2);
        }

        [Fact]
        public void Should_Compare_Properly()
        {
            MultiLanguageString mls1 = MultiLanguageString.New(Cultures.GermanGermany, "de");
            MultiLanguageString mls2 = MultiLanguageString.New(Cultures.EnglishUnitedStates, "us");

            Assert.Equal(0, mls1.CompareTo(mls1));
            Assert.InRange(mls1.CompareTo(mls2), int.MinValue, -1);
            Assert.InRange(mls2.CompareTo(mls1), 1, int.MaxValue);
        }

        [Fact]
        public void Clone_Should_Create_New_Instance()
        {
            MultiLanguageString original = MultiLanguageString.New(Cultures.GermanGermany, "de");

            MultiLanguageString clone = original.Clone();

            Assert.NotSame(original, clone);
        }

        [Fact]
        public void Cloned_String_Should_Equal_Original()
        {
            MultiLanguageString original = MultiLanguageString.New(Cultures.GermanGermany, "de");

            MultiLanguageString clone = original.Clone();

            Assert.Equal(original, clone);
        }

        [Fact]
        public void Should_Find_Specific_Translation()
        {
            MultiLanguageString mls = MultiLanguageString.New(Cultures.GermanGermany, "specific");

            Assert.Equal("specific", mls.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Find_Neutral_Translation_As_Fallback()
        {
            MultiLanguageString mls = MultiLanguageString.New(Cultures.GermanNeutral, "neutral");

            Assert.Equal("neutral", mls.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Find_Invariant_Translation_As_Fallback()
        {
            MultiLanguageString mls = MultiLanguageString.New(CultureInfo.InvariantCulture, "invariant");

            Assert.Equal("invariant", mls.ToString(Cultures.GermanGermany));
        }

        [Fact]
        public void Should_Use_Single_Existing_Translation_As_Fallback()
        {
            MultiLanguageString mls = MultiLanguageString.New(Cultures.EnglishUnitedStates, "single");

            Assert.Equal("single", mls.ToString(Cultures.GermanGermany));
        }
    }
}
