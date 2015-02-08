namespace Hydra.Infrastructure.Test.Configuration
{
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class DefaultSettingsTests
    {
        [Fact]
        public void A_Default_Setting_Should_Exist_For_Each_Known_Setting_Name()
        {
            var actual = DefaultSettings.All();

            var expected = KnownSettingNames.All();

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[i], actual[i].Name);
            }
        }
    }
}