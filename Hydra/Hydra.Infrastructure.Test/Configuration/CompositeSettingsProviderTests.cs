namespace Hydra.Infrastructure.Test.Configuration
{
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class CompositeSettingsProviderTests
    {
        [Fact]
        public void Should_Merge_Settings_From_All_Providers_Last_One_Wins()
        {
            SettingName sn1, sn2, sn3, sn4;
            Assert.True(SettingName.TryParse("Hydra.1", out sn1));
            Assert.True(SettingName.TryParse("Hydra.2", out sn2));
            Assert.True(SettingName.TryParse("Hydra.3", out sn3));
            Assert.True(SettingName.TryParse("Hydra.4", out sn4));

            ISettingsProvider sp1 = new InMemorySettingsProvider(new Setting(sn1, "1"), new Setting(sn4, "14"));
            ISettingsProvider sp2 = new InMemorySettingsProvider(new Setting(sn2, "2"), new Setting(sn4, "24"));
            ISettingsProvider sp3 = new InMemorySettingsProvider(new Setting(sn3, "3"), new Setting(sn4, "34"));

            ISettingsProvider sut = new CompositeSettingsProvider(sp1, sp2, sp3);

            SettingsCollection actual = sut.GetSettings();

            Assert.Equal(4, actual.Count);
            Assert.Equal("1", actual[sn1].Value);
            Assert.Equal("2", actual[sn2].Value);
            Assert.Equal("3", actual[sn3].Value);
            Assert.Equal("34", actual[sn4].Value);
        }
    }
}
