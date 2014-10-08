namespace Hydra.Test.Hosting
{
    using System.IO;
    using System.Reflection;
    using System.Web.Hosting;
    using Hydra.Hosting;
    using Xunit;

    public class EmbeddedFileTests
    {
        [Fact]
        public void Should_Open_Resource_Stream()
        {
            string fileName = "~/Texts/Long/VeryLong.txt";
            Assembly assembly = this.GetType().Assembly;

            VirtualFile file = new EmbeddedFile(fileName, "Hydra.Test.Texts.Long.VeryLong.txt", assembly);

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