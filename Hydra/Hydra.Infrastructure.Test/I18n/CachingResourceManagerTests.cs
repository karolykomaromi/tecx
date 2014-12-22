namespace Hydra.Infrastructure.Test.I18n
{
    using System.Globalization;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class CachingResourceManagerTests
    {
        [Fact]
        public void Should_Dispose_InvalidationTokens_When_Disposed()
        {
            ObjectCache cache = new MemoryCache("TESTS");

            CultureInfo culture = Cultures.GermanGermany;

            string s1, s2;

            using (var sut = new CachingResourceManager("CachingResourceManagerTests", cache))
            {
                s1 = sut.GetString("Foo", culture);
                s2 = sut.GetString("Bar", culture);

                Assert.Equal(cache[s1], s1);
                Assert.Equal(cache[s2], s2);
            }

            Assert.Null(cache[s1]);
            Assert.Null(cache[s2]);
        }
    }
}
