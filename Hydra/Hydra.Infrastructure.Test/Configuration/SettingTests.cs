namespace Hydra.Infrastructure.Test.Configuration
{
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class SettingTests
    {
        [Fact]
        public void Should_Equal_When_Names_And_Values_Are_Equal()
        {
            SettingName sn = KnownSettingNames.Hydra.Infrastructure.Configuration.Test;

            Setting s1 = new Setting(sn, 1);
            Setting s2 = new Setting(sn, 1);

            Assert.Equal(s1, s2);
            Assert.Equal(s2, s1);
        }
    }
}