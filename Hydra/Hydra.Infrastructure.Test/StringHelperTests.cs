namespace Hydra.Infrastructure.Test
{
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Xunit;

    public class StringHelperTests
    {
        [Fact]
        public void Should_Split_Word_At_Camel_Humps()
        {
            string actual = StringHelper.SplitCamelCase("NonAuthoritativeInformation");

            string expected = "Non Authoritative Information";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Leave_Acronyms_Intact()
        {
            string actual = StringHelper.SplitCamelCase("ProductID");

            string expected = "Product ID";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Write_String_To_File()
        {
            string expected = "Should_Write_String_To_File";

            string fileName = "Should_Write_String_To_File.txt";

            expected.SaveToFile(fileName);

            using (Stream stream = File.OpenRead(fileName))
            {
                using (TextReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string actual = reader.ReadToEnd();

                    Assert.Equal(expected, actual);
                }
            }
        }

        [Fact]
        public void Should_Convert_First_Character_To_UpperCase()
        {
            Assert.Equal("Upper", StringHelper.CapitalizeFirstLetter("upper"));
        }

        [Fact]
        public void Should_Convert_Single_Character_To_UpperCase()
        {
            Assert.Equal("U", StringHelper.CapitalizeFirstLetter("u"));
        }

        [Fact]
        public void Should_Never_Return_Null()
        {
            Assert.Equal(string.Empty, StringHelper.CapitalizeFirstLetter("    "));
            Assert.Equal(string.Empty, StringHelper.CapitalizeFirstLetter(string.Empty));
            Assert.Equal(string.Empty, StringHelper.CapitalizeFirstLetter(null));
        }

        [Fact]
        public void Should_Generate_Valid_Property_Name()
        {
            CultureInfo culture = new CultureInfo(1071); // 47   1071

            string actual = StringHelper.ToValidPropertyName(culture.EnglishName);

            string expected = "Macedonian_Former_Yugoslav_Republic_of_Macedonia";

            Assert.Equal(expected, actual);
        }
    }
}
