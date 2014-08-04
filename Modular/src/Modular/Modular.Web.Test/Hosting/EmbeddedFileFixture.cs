namespace Modular.Web.Test.Hosting
{
    using System.IO;
    using System.Reflection;
    using System.Web.Hosting;
    using Modular.Web.Hosting;
    using Xunit;

    public class EmbeddedFileFixture
    {
        [Fact]
        public void Should_Open_Resource_Stream()
        {
            string fileName = "~/Texts/Long/VeryLong.txt";
            Assembly assembly = this.GetType().Assembly;
            ////var virtualPathUtility = new Mock<IVirtualPathUtility>();
            ////virtualPathUtility.Setup(vpu => vpu.ToAppRelative(fileName)).Returns(fileName);
            ////var provider = EmbeddedResourceVirtualPathProvider.Create(virtualPathUtility.Object, assembly);

            VirtualFile file = new EmbeddedFile(fileName, "Modular.Web.Test.Assets.Texts.Long.VeryLong.txt", assembly);

            using(Stream stream = file.Open())
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