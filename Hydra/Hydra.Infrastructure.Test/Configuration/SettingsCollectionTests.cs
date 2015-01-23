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

        [Fact]
        public void Should_Merge_Collections_Letting_Second_Collection_Win()
        {
            SettingName sn;
            Assert.True(SettingName.TryParse("Hydra.Foo", out sn));

            SettingsCollection sc1 = new SettingsCollection(new Setting(sn, 1));
            SettingsCollection sc2 = new SettingsCollection(new Setting(sn, 2));

            SettingsCollection actual = sc1.Merge(sc2);

            Assert.Equal(2, actual[sn].Value);
        }
    }
}
