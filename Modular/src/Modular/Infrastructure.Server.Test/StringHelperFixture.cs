namespace Infrastructure.Server.Test
{
    using Xunit;

    public class StringHelperFixture
    {
        [Fact]
        public void Should_Convert_Lower_To_Upper_Camel_Case()
        {
            string s = "lower";

            string expected = "Lower";

            string actual = StringHelper.ToUpperCamelCase(s);

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void Should_Convert_Single_Character_Strings_To_Upper_Camel_Case()
        {
            string s = "c";

            string expected = "C";

            string actual = StringHelper.ToUpperCamelCase(s);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Convert_Shout_Case_To_Upper_Camel_Case()
        {
            string s = "SHOUT";

            string expected = "Shout";

            string actual = StringHelper.ToUpperCamelCase(s);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Convert_DotSeparated_Strings_To_Upper_Camel_Case()
        {
            string s = "SHOUT.OUT.LOUD";

            string expected = "Shout.Out.Loud";

            string actual = StringHelper.ToUpperCamelCase(s);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Convert_UnderscoreSeparated_Strings_To_Upper_Camel_Case_And_Remove_Underscores()
        {
            string s = "SHOUT_OUT_LOUD";

            string expected = "ShoutOutLoud";

            string actual = StringHelper.ToUpperCamelCase(s);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Convert_First_Character_To_Lower_Invariant()
        {
            string s = "Upper";

            string expected = "upper";

            string actual = StringHelper.FirstCharacterToLowerInvariant(s);

            Assert.Equal(expected, actual);
        }
    }
}
