namespace Modular.Test
{
    using Infrastructure;
    using Infrastructure.Reflection;
    using Main.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Recipe.ViewModels;
    using Search;

    [TestClass]
    public class DefaultNamespaceFixture
    {
        [TestMethod]
        public void Should_Set_Correct_Default_Namespaces()
        {
            Assert.AreEqual("Modular", ReflectionHelper.GetDefaultNamespace(typeof(Shell).Assembly));
            Assert.AreEqual("Infrastructure", ReflectionHelper.GetDefaultNamespace(typeof(TypeHelper).Assembly));
            Assert.AreEqual("Main", ReflectionHelper.GetDefaultNamespace(typeof(ChangeLanguageCommand).Assembly));
            Assert.AreEqual("Recipe", ReflectionHelper.GetDefaultNamespace(typeof(IngredientDetailsViewModel).Assembly));
            Assert.AreEqual("Search", ReflectionHelper.GetDefaultNamespace(typeof(SearchServiceClient).Assembly));
        }
    }
}
