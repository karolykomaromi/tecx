namespace Hydra.Infrastructure.Test.Configuration
{
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class KnownSettingNamesTests
    {
        [Fact]
        public void A_Known_Setting_Name_Should_Exist_For_Each_Default_Setting()
        {
            var actual = KnownSettingNames.All();

            var expected = DefaultSettings.All();

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[i].Name, actual[i]);
            }
        }
    }
}
