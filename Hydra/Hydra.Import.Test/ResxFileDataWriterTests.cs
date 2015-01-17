namespace Hydra.Import.Test
{
    using System.IO;
    using System.Resources;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class ResXFileDataWriterTests
    {
        [Fact]
        public void Should_Correctly_Write_ResourceItem_To_File()
        {
            using (Stream stream = new FileStream(@".\Hydra.Import.Test.MyResX.de.resx", FileMode.Create))
            {
                var writer = new ResXFileDataWriter(stream);

                var resourceItems = new[] { new ResourceItem { Name = "Foo", Value = "1" } };

                writer.Write(resourceItems);
            }

            var resourceManager = ResourceManager.CreateFileBasedResourceManager("Hydra.Import.Test.MyResX", ".\\", typeof(ResXResourceSet));

            string actual = resourceManager.GetString("Foo", Cultures.GermanNeutral);

            string expected = "1";

            Assert.Equal(expected, actual);
        }
    }
}
