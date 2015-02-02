namespace Hydra.Infrastructure.Test.Configuration
{
    using System.ComponentModel;
    using Hydra.Infrastructure.Configuration;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class SettingElementTypeConverterTests
    {
        [Fact]
        public void Should_Convert_Setting_With_Primitive_Value_To_SettingElement()
        {
            Setting s = new Setting(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, 1);

            TypeConverter sut = new SettingElementTypeConverter();

            Assert.True(sut.CanConvertFrom(typeof(Setting)));

            SettingElement actual = sut.ConvertFrom(null, Cultures.GermanNeutral, s) as SettingElement;

            Assert.NotNull(actual);
            Assert.Equal(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, actual.SettingName);
            Assert.Equal(typeof(int), actual.Type);
            Assert.Equal("1", actual.Value);
        }

        [Fact]
        public void Should_Convert_Setting_With_Complex_Value_To_SettingElement()
        {
            Setting s = new Setting(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, new Foo { Bar = 123, Baz = new Baz { Bazz = "456" } });

            TypeConverter sut = new SettingElementTypeConverter();

            Assert.True(sut.CanConvertFrom(typeof(Setting)));

            SettingElement actual = sut.ConvertFrom(null, Cultures.GermanNeutral, s) as SettingElement;

            string expected = "<Foo>\r\n  <Baz>\r\n    <Bazz>456</Bazz>\r\n  </Baz>\r\n  <Bar>123</Bar>\r\n</Foo>";

            Assert.NotNull(actual);
            Assert.Equal(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, actual.SettingName);
            Assert.Equal(typeof(Foo), actual.Type);
            Assert.Equal(expected, actual.Value);
        }

        [Fact]
        public void Should_Convert_SettingElement_With_Primitive_Value_To_Setting()
        {
            SettingElement se = new SettingElement
                {
                    SettingName = KnownSettingNames.Hydra.Infrastructure.Configuration.Test,
                    Type = typeof(int),
                    Value = "1"
                };

            TypeConverter sut = new SettingElementTypeConverter();

            Assert.True(sut.CanConvertTo(typeof(Setting)));

            Setting actual = sut.ConvertTo(null, Cultures.GermanNeutral, se, typeof(Setting)) as Setting;

            Assert.NotNull(actual);
            Assert.Equal(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, actual.Name);
            Assert.Equal(1, actual.Value);
        }

        [Fact]
        public void Should_Convert_SettingElement_With_Complex_Value_To_Setting()
        {
            SettingElement se = new SettingElement
            {
                SettingName = KnownSettingNames.Hydra.Infrastructure.Configuration.Test,
                Type = typeof(Foo),
                Value = "<Foo><Baz><Bazz>456</Bazz></Baz><Bar>123</Bar></Foo>"
            };

            TypeConverter sut = new SettingElementTypeConverter();

            Assert.True(sut.CanConvertTo(typeof(Setting)));

            Setting actual = sut.ConvertTo(null, Cultures.GermanNeutral, se, typeof(Setting)) as Setting;

            Assert.NotNull(actual);
            Assert.Equal(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, actual.Name);

            Foo foo = Assert.IsType<Foo>(actual.Value);

            Assert.Equal(123, foo.Bar);
            Assert.NotNull(foo.Baz);
            Assert.Equal("456", foo.Baz.Bazz);
        }

        public class Foo
        {
            public Baz Baz { get; set; }

            public int Bar { get; set; }
        }

        public class Baz
        {
            public string Bazz { get; set; }
        }
    }
}
