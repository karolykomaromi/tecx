namespace Infrastructure.Server.Test
{
    using Infrastructure.ListViews;
    using Xunit;

    public class ResourceKeyProviderFixture
    {
        [Fact]
        public void Should_Create_Correct_Key()
        {
            IResourceKeyProvider provider = new ResourceKeyProvider();

            string expected = "Recipe.Foo";

            string actual = provider.GetResourceKey(new ListViewId("RECIPE", "RECIPES"), "Foo");

            Assert.Equal(expected, actual);
        }
    }
}
