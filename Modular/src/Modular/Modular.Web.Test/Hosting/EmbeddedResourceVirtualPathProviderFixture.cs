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

            var provider = EmbeddedResourceVirtualPathProvider.Create(assembly, virtualPathUtility.Object);

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
    }
}
