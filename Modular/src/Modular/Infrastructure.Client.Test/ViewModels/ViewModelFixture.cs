namespace Infrastructure.Client.Test.ViewModels
{
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.I18n;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewModelFixture
    {
        [TestMethod]
        public void Should_Request_Correct_Resource()
        {
            var resourceManager = new Mock<IResourceManager>();
            resourceManager.SetupGet(rm => rm[new ResxKey("INFRASTRUCTURE.CLIENT.TEST.TESTOBJECTS.MULTILANGUAGE.FOO")]).Returns("Foo");

            var vm = new MultiLanguageViewModel { ResourceManager = resourceManager.Object };

            string actual = vm.Foo;

            Assert.AreEqual("Foo", actual);
        }

        [TestMethod]
        public void Should_Insert_Underscore_If_PropertyName_StartsWith_Label()
        {
            var resourceManager = new Mock<IResourceManager>();
            resourceManager.SetupGet(rm => rm[new ResxKey("INFRASTRUCTURE.CLIENT.TEST.TESTOBJECTS.MULTILANGUAGE.LABEL_FOO")]).Returns("Foo");

            var vm = new MultiLanguageViewModel { ResourceManager = resourceManager.Object };

            string actual = vm.LabelFoo;

            Assert.AreEqual("Foo", actual);
        }
    }
}
