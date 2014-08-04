namespace Modular.Web.Test
{
    using Infrastructure;
    using Infrastructure.Reflection;
    using Modular.Web.Hosting;
    using Recipe;
    using Search;
    using Xunit;

    public class DefaultNamespaceFixture
    {
        [Fact]
        public void Should_Set_Correct_DefaultNamespaces()
        {
            Assert.Equal("Modular.Web", ReflectionHelper.GetDefaultNamespace(typeof(UnityServiceHostFactory).Assembly));
            Assert.Equal("Infrastructure", ReflectionHelper.GetDefaultNamespace(typeof(TypeHelper).Assembly));
            Assert.Equal("Recipe", ReflectionHelper.GetDefaultNamespace(typeof(IRecipeService).Assembly));
            Assert.Equal("Search", ReflectionHelper.GetDefaultNamespace(typeof(ISearchService).Assembly));
        }
    }
}
