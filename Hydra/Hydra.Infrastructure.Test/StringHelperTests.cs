namespace Hydra.Infrastructure.Test
{
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
    }
}
