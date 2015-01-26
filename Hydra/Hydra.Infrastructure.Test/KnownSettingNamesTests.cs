namespace Hydra.Infrastructure.Test
{
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class KnownSettingNamesTests
    {
        [Fact]
        public void Should_Get_Setting_Name_From_Expression()
        {
            Assert.Equal("HYDRA.IMPORT.DUMMY", KnownSettingNames.Hydra.Import.Dummy.FullName);
        }
    }
}
