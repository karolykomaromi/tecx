namespace Hydra.Infrastructure.Test.Configuration
{
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class SettingsCollectionTests
    {
        [Fact]
        public void Should_Return_Empty_Setting_On_Unknown_Key()
        {
            SettingName sn;
            Assert.True(SettingName.TryParse("Hydra.Foo", out sn));

            SettingsCollection sut = new SettingsCollection();

            Assert.Same(Setting.Empty, sut[sn]);
        }
    }
}
