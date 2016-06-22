namespace Cars.Test.I18n
{
    using Cars.I18n;
    using Xunit;

    public class PolyglotStringBuilderTests
    {
        [Fact]
        public void Should_Add_Translations()
        {
            var sut = new PolyglotStringBuilder()
                .GermanGermany("de-DE")
                .EnglishUnitedKingdom("en-GB");

            PolyglotString actual = sut.Build();

            Assert.Equal(actual.ToString(Cultures.GermanGermany), "de-DE");
            Assert.Equal(actual.ToString(Cultures.EnglishUnitedKingdom), "en-GB");
        }

        [Fact]
        public void Should_Overwrite_Translation_Without_Throwing()
        {
            var sut = new PolyglotStringBuilder()
                .GermanGermany("1")
                .GermanGermany("2");

            PolyglotString actual = sut.Build();

            Assert.Equal(actual.ToString(Cultures.GermanGermany), "2");
        }

        [Fact]
        public void Should_Return_Empty_When_No_Translations()
        {
            var sut = new PolyglotStringBuilder();

            PolyglotString actual = sut.Build();

            Assert.Same(PolyglotString.Empty, actual);
        }
    }
}
