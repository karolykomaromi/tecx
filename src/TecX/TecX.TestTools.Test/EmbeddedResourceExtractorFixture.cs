namespace TecX.TestTools.Test
{
    using System.IO;

    using Xunit;

    public class EmbeddedResourceExtractorFixture
    {
        [Fact]
        public void Should_Extract_Simple_Resource()
        {
            var extractor = new EmbeddedResourceExtractor();

            extractor.Extract();

            FileInfo pic01 = new FileInfo("Resources\\Pic01.png");

            Assert.True(pic01.Exists);
        }

        [Fact]
        public void Should_Extract_Resource_In_Subfolder()
        {
            var extractor = new EmbeddedResourceExtractor();

            extractor.Extract();

            FileInfo pic02 = new FileInfo("Resources\\Sub01\\Pic02.png");

            Assert.True(pic02.Exists);
        }

        [Fact]
        public void Should_Extract_Resource_In_Subfolder_Hierarchy()
        {
            var extractor = new EmbeddedResourceExtractor();

            extractor.Extract();

            FileInfo pic03 = new FileInfo("Resources\\Sub01\\Sub02\\Pic03.png");

            Assert.True(pic03.Exists);
        }
    }
}
