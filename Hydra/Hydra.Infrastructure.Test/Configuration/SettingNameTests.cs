namespace Hydra.Infrastructure.Test.Configuration
{
    using System;
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class SettingNameTests
    {
        [Fact]
        public void Should_Parse_Valid_String()
        {
            string s = "Hydra.Foo";

            SettingName actual;
            Assert.True(SettingName.TryParse(s, out actual));
            Assert.Equal("HYDRA", actual.Group);
            Assert.Equal("FOO", actual.Name);
            Assert.Equal("HYDRA.FOO", actual.FullName, StringComparer.Ordinal);
        }

        [Fact]
        public void Should_Not_Parse_Invalid_String()
        {
            string s = "Hydra";

            SettingName actual;
            Assert.False(SettingName.TryParse(s, out actual));
            Assert.Same(SettingName.Empty, actual);
        }

        [Fact]
        public void Should_Recognize_Equal_Names()
        {
            string s1 = "Hydra.Foo";
            string s2 = "Hydra.Foo";

            SettingName sn1, sn2;
            Assert.True(SettingName.TryParse(s1, out sn1));
            Assert.True(SettingName.TryParse(s2, out sn2));

            Assert.Equal(sn1, sn2);
        }

        [Fact]
        public void Should_Compare_Correctly()
        {
            SettingName sn1, sn2;
            Assert.True(SettingName.TryParse("Hydra.1", out sn1));
            Assert.True(SettingName.TryParse("Hydra.2", out sn2));

            Assert.InRange(sn1.CompareTo(sn2), int.MinValue, -1);
            Assert.InRange(sn2.CompareTo(sn1), 1, int.MaxValue);
            Assert.Equal(0, sn1.CompareTo(sn1));
        }

        [Fact]
        public void Should_Clone_Correctly()
        {
            SettingName original;

            Assert.True(SettingName.TryParse("Hydra.Foo", out original));

            SettingName clone = original.Clone();

            Assert.NotSame(original, clone);
            Assert.Equal(original, clone);
        }
    }
}
