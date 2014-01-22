namespace Infrastructure.Client.Test.I18n
{
    using Infrastructure.Client.Test.Assets.Resources;
    using Infrastructure.I18n;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ResxFileResourceManagerFixture
    {
        [TestMethod]
        public void Should_Return_Matching_String()
        {
            IResourceManager resourceManager = new ResxFilesResourceManager(typeof(Labels));

            string key = "INFRASTRUCTURE.TRANSLATEME";

            string actual = resourceManager[key];

            Assert.AreEqual("english", actual);
        }
    }
}
