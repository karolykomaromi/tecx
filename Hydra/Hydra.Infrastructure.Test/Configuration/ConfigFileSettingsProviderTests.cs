namespace Hydra.Infrastructure.Test.Configuration
{
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class ConfigFileSettingsProviderTests
    {
        [Fact]
        public void Should_Load_Settings_From_Config_File()
        {
            ISettingsProvider sut = new AppConfigSettingsProvider();

            SettingsCollection actual = sut.GetSettings();

            Assert.Equal(1, actual.Count);

            Setting s = actual[KnownSettingNames.Hydra.Infrastructure.Configuration.Test];

            Assert.NotEqual(Setting.Empty, s);
            Assert.Equal(1, s.Value);
        }
    }
}
