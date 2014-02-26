namespace Infrastructure.Server.Test
{
    using Xunit;

    public class ResourceKeyProviderFixture
    {
        [Fact]
        public void Should_Create_Correct_Key()
        {
            IResourceKeyProvider provider = new ResourceKeyProvider();

            string expected = "Recipe.Foo";

            string actual = provider.GetResourceKey("RECIPE.RECIPES", "Foo");

            Assert.Equal(expected, actual);
        }
    }
}
