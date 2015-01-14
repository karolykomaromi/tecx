namespace Hydra.Infrastructure.Test.Configuration
{
    using System.Configuration;
    using System.Linq;
    using Hydra.Infrastructure.Configuration;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class HydraConfigurationSectionGroupTests
    {
        [Fact]
        public void Should_Read_Supported_Languages_From_Config_File()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

            HydraConfigurationSectionGroup group = HydraConfigurationSectionGroup.HydraConfiguration(configuration);

            Assert.NotNull(group);
            Assert.NotNull(group.ApplicationSettings);
            Assert.NotNull(group.ApplicationSettings.SupportedLanguages);
            Assert.Equal(2, group.ApplicationSettings.SupportedLanguages.Count);
            Assert.Equal(Cultures.GermanNeutral, group.ApplicationSettings.SupportedLanguages.First().Culture);
            Assert.Equal(Cultures.GermanGermany, group.ApplicationSettings.SupportedLanguages.ElementAt(1).Culture);
        }
    }
}
