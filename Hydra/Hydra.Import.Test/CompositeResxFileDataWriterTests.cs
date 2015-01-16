namespace Hydra.Import.Test
{
    using System.Resources;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class CompositeResxFileDataWriterTests
    {
        [Fact]
        public void Should()
        {
            using (var writer = new CompositeResxFileDataWriter("Hydra.Import.Test.CompositeResx", ".\\"))
            {
                var resourceItems = new[]
                    {
                        new ResourceItem { Name = "Foo", Value = "1", Language = Cultures.GermanNeutral} ,
                        new ResourceItem { Name = "Foo", Value = "2", Language = Cultures.EnglishNeutral }
                    };

                writer.Write(resourceItems);
            }

            var resourceManager = ResourceManager.CreateFileBasedResourceManager("Hydra.Import.Test.CompositeResx", ".\\", typeof(ResXResourceSet));

            Assert.Equal("1", resourceManager.GetString("Foo", Cultures.GermanNeutral));

            Assert.Equal("2", resourceManager.GetString("Foo", Cultures.EnglishNeutral));
        }
    }
}