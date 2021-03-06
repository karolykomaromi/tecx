﻿namespace Hydra.Infrastructure.Test.Configuration
{
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class PersistentSettingTypeConverterTests
    {
        [Fact]
        public void Should_Convert_PersistentSetting_To_Setting()
        {
            byte[] blob;

            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, 1);

                blob = stream.GetBuffer();
            }

            PersistentSetting ps = new PersistentSetting
            {
                Name = KnownSettingNames.Hydra.Infrastructure.Configuration.Test.FullName,
                Value = blob
            };

            TypeConverter sut = new PersistentSettingTypeConverter();

            Assert.True(sut.CanConvertTo(typeof(Setting)));

            Setting actual = sut.ConvertTo(ps, typeof(Setting)) as Setting;

            Assert.NotNull(actual);
            Assert.Equal(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, actual.Name);
            Assert.Equal(1, actual.Value);
        }

        [Fact]
        public void Should_Convert_Setting_To_PersistentSetting()
        {
            Setting s = new Setting(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, 1);

            TypeConverter sut = new PersistentSettingTypeConverter();

            Assert.True(sut.CanConvertFrom(typeof(Setting)));

            PersistentSetting actual = sut.ConvertFrom(s) as PersistentSetting;

            Assert.NotNull(actual);
            Assert.Equal(KnownSettingNames.Hydra.Infrastructure.Configuration.Test.FullName, actual.Name);

            byte[] blob;
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, 1);
                blob = stream.GetBuffer();
            }

            Assert.Equal(actual.Value, blob);
        }
    }
}
