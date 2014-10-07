namespace Hydra.Test.Hosting
{
    using System.Reflection;
    using Hydra.Hosting;
    using Hydra.Theme.Blue;
    using Moq;
    using Xunit;

    public class EmbeddedResourceVirtualPathProviderTests
    {
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
        public void Should_Find_Existing_Files_From_Multiple_Assemblies()
        {
            string css = "~/Content/Site.css";
            string red = "~/Images/Red.png";

            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(It.IsAny<string>()))
                              .Returns((string virtualPath) => virtualPath);

            var assemblies = new[] { this.GetType().Assembly, typeof(Blue).Assembly };

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assemblies);

            Assert.True(provider.FileExists(css));
            Assert.True(provider.FileExists(red));
        }

        [Fact]
        public void Should_Not_Find_Non_Existing_File()
        {
            string fileName = "~/Texts/Foo.txt";
            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(fileName)).Returns(fileName);

            Assembly assembly = this.GetType().Assembly;

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assembly);

            Assert.False(provider.FileExists(fileName));
        }

        [Fact]
        public void Should_Return_Null_If_File_Does_Not_Exist()
        {
            string fileName = "~/Texts/Foo.txt";
            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(fileName)).Returns(fileName);

            Assembly assembly = this.GetType().Assembly;

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assembly);

            Assert.Null(provider.GetFile(fileName));
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
        public void Should_Not_Find_Non_Existing_Directory()
        {
            string dirName = "~/Texts/Foo/";
            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(dirName)).Returns(dirName);

            Assembly assembly = this.GetType().Assembly;

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assembly);

            Assert.False(provider.DirectoryExists(dirName));
        }

        [Fact]
        public void Should_Return_Null_If_Directory_Does_Not_Exist()
        {
            string dirName = "~/Texts/Foo/";
            var virtualPathUtility = new Mock<IVirtualPathUtility>();
            virtualPathUtility.Setup(vpu => vpu.ToAppRelative(dirName)).Returns(dirName);

            Assembly assembly = this.GetType().Assembly;

            var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assembly);

            Assert.Null(provider.GetDirectory(dirName));
        }
    }
}