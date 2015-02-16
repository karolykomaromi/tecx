namespace Hydra.Import.Test.Data
{
    using System.Resources;
    using Hydra.Import.Data;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class CompositeResXFileDataWriterTests
    {
        [Fact]
        public void Should()
        {
            using (var writer = new CompositeResXFileDataWriter("Hydra.Import.Test.CompositeResX", ".\\"))
            {
                var resourceItems = new[]
                    {
                        new ResourceItem { Name = "Foo", Value = "1", Language = Cultures.GermanNeutral },
                        new ResourceItem { Name = "Foo", Value = "2", Language = Cultures.EnglishNeutral }
                    };

                writer.Write(resourceItems);
            }

            var resourceManager = ResourceManager.CreateFileBasedResourceManager("Hydra.Import.Test.CompositeResX", ".\\", typeof(ResXResourceSet));

            Assert.Equal("1", resourceManager.GetString("Foo", Cultures.GermanNeutral));

            Assert.Equal("2", resourceManager.GetString("Foo", Cultures.EnglishNeutral));
        }
    }
}