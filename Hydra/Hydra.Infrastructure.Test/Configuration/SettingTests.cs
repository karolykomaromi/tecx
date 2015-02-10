﻿namespace Hydra.Infrastructure.Test.Configuration
{
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class SettingTests
    {
        [Fact]
        public void Should_Equal_When_Both_Values_Are_Null()
        {
            SettingName sn;
            Assert.True(SettingName.TryParse("Hydra.Foo", out sn));

            Setting s1 = new Setting(sn, null);
            Setting s2 = new Setting(sn, null);

            Assert.Equal(s1, s2);
        }

        [Fact]
        public void Should_Not_Equal_When_Either_Value_Is_Null()
        {
            SettingName sn;
            Assert.True(SettingName.TryParse("Hydra.Foo", out sn));

            Setting s1 = new Setting(sn, null);
            Setting s2 = new Setting(sn, 1);

            Assert.NotEqual(s1, s2);
            Assert.NotEqual(s2, s1);
        }

        [Fact]
        public void Should_Equal_When_Names_And_Values_Are_Equal()
        {
            SettingName sn;
            Assert.True(SettingName.TryParse("Hydra.Foo", out sn));

            Setting s1 = new Setting(sn, 1);
            Setting s2 = new Setting(sn, 1);

            Assert.Equal(s1, s2);
            Assert.Equal(s2, s1);
        }
    }
}