namespace Hydra.Infrastructure.Test.Configuration
{
    using System.Globalization;
    using Hydra.Infrastructure.Configuration;
    using NHibernate;
    using Xunit;
    using Xunit.Extensions;

    public class NhSettingsProviderTests
    {
        [Theory, ContainerData]
        public void Should_Load_Settings_From_Session(ISession session)
        {
            using (session)
            {
                using (var tx = session.BeginTransaction())
                {
                    Setting s = new Setting(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, 1);

                    PersistentSetting ps = (PersistentSetting)ConvertHelper.Convert(s, typeof(PersistentSetting), CultureInfo.InvariantCulture);

                    session.Save(ps);

                    ISettingsProvider sut = new NhSettingsProvider(session);

                    SettingsCollection actual = sut.GetSettings();

                    Assert.Equal(1, actual.Count);

                    Assert.Equal(s, actual[KnownSettingNames.Hydra.Infrastructure.Configuration.Test]);
                }
            }
        }
    }
}
