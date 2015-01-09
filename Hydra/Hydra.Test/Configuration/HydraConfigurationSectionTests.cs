namespace Hydra.Test.Configuration
{
    using System.Configuration;
    using System.Linq;
    using Hydra.Configuration;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class HydraConfigurationSectionTests
    {
        [Fact]
        public void Should_Read_Supported_Languages_From_Config_File()
        {
            HydraConfigurationSectionGroup group = (HydraConfigurationSectionGroup)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("hydra");

            Assert.NotNull(group);
            Assert.NotNull(group.ApplicationSettings);
            Assert.NotNull(group.ApplicationSettings.SupportedLanguages);
            Assert.Equal(2, group.ApplicationSettings.SupportedLanguages.Count);
            Assert.Equal(Cultures.GermanNeutral, group.ApplicationSettings.SupportedLanguages.OfType<SupportedLanguage>().First().Culture);
            Assert.Equal(Cultures.GermanGermany, group.ApplicationSettings.SupportedLanguages.OfType<SupportedLanguage>().ElementAt(1).Culture);
        }
    }
}
