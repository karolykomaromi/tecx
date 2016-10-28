using System.IO;
using Xunit;

namespace Janus.TextTemplating.Test
{
    public class ResourcesTemplateHelperTests
    {
        [Fact]
        public void Should_Parse_Simple_String_Property()
        {
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Janus.TextTemplating.Test.Resources.StringResources.xml"))
            {
                ResourcesTemplate sut = ResourcesTemplate.FromStream(stream);

                string actual = sut.Properties();

                string expected = "\r\n        public static string MyString\r\n        {\r\n            get\r\n            {\r\n                return ResourceManager.GetString(\"MyString\", resourceCulture);\r\n            }\r\n        }\r\n";

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Should_Parse_Text_File_As_String_Property()
        {
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Janus.TextTemplating.Test.Resources.TextFileResources.xml"))
            {
                ResourcesTemplate sut = ResourcesTemplate.FromStream(stream);

                string actual = sut.Properties();

                string expected = "\r\n        public static string MyTextFile\r\n        {\r\n            get\r\n            {\r\n                return ResourceManager.GetString(\"MyTextFile\", resourceCulture);\r\n            }\r\n        }\r\n";

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Should_Parse_Any_Other_File_As_Byte_Array_Property()
        {
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Janus.TextTemplating.Test.Resources.SomeOtherFileResources.xml"))
            {
                ResourcesTemplate sut = ResourcesTemplate.FromStream(stream);

                string actual = sut.Properties();

                string expected = "\r\n        public static byte[] MyJsonFile\r\n        {\r\n            get\r\n            {\r\n                object obj = ResourceManager.GetObject(\"MyJsonFile\", resourceCulture);\r\n                return (byte[])obj;\r\n            }\r\n        }\r\n";

                Assert.Equal(expected, actual);
            }
        }
    }
}
