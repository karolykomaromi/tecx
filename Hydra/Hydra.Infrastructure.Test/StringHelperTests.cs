namespace Hydra.Infrastructure.Test
{
    using System.IO;
    using System.Linq;
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
        public void Should_Chunkify_Empty_String_As_Empty_String()
        {
            Assert.NotNull(StringHelper.Chunkify(string.Empty, 1));
        }

        [Fact]
        public void Should_Chunkify_String_That_Is_Shorter_Than_Chunk_Size()
        {
            string s = "12345";

            Assert.Equal(s, StringHelper.Chunkify(s, 15).Single());
        }
    }
}