namespace Modular.Web.Test.Hosting
{
    using System.IO;
    using System.Reflection;
    using System.Web.Hosting;
    using Modular.Web.Hosting;
    using Moq;
    using Xunit;

    public class EmbeddedResourceVirtualPathProviderFixture
    {
        [Fact]
        public void Should_Open_Resource_Stream()
        {
            string fileName = "~/Texts/Long/VeryLong.txt";
            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(fileName)).Returns(fileName);

            Assembly assembly = this.GetType().Assembly;

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assembly);

            VirtualFile file = provider.GetFile(fileName);

            using (Stream stream = file.Open())
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    string s = reader.ReadToEnd();

                    Assert.Equal("This is a very long text.", s);
                }
            }
        }

        [Fact]
        public void Should_Find_Existing_File()
        {
            string fileName = "~/Texts/Long/VeryLong.txt";
            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(fileName)).Returns(fileName);

            Assembly assembly = this.GetType().Assembly;

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assembly);

            Assert.True(provider.FileExists(fileName));
        }

        [Fact]
        public void Should_Find_Existing_Directory()
        {
            string dirName = "~/Texts/Long/";
            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(dirName)).Returns(dirName);

            Assembly assembly = this.GetType().Assembly;

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assembly);

            Assert.True(provider.DirectoryExists(dirName));
        }

        [Fact]
        public void Should_Find_Existing_Files_From_Multiple_Assemblies()
        {
            string blue = "~/Images/Blue.png";
            string red = "~/Images/Red.png";

            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(It.IsAny<string>()))
                              .Returns((string virtualPath) => virtualPath);

            var assemblies = new[] { this.GetType().Assembly, typeof(UnityServiceHostFactory).Assembly };

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assemblies);

            Assert.True(provider.FileExists(blue));
            Assert.True(provider.FileExists(red));
        }
    }
}
