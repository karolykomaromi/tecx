namespace Infrastructure.Client.Test.I18n
{
    using System.Globalization;
    using System.Threading;
    using Infrastructure.Client.Test.Assets.Resources;
    using Infrastructure.I18n;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ResxFileResourceManagerFixture
    {
        private CultureInfo currentCulture;
        private CultureInfo currentUICulture;

        [TestInitialize]
        public void Initialize()
        {
            this.currentCulture = Thread.CurrentThread.CurrentCulture;
            this.currentUICulture = Thread.CurrentThread.CurrentUICulture;
        }

        [TestCleanup]
        public void Cleanup()
        {
            Thread.CurrentThread.CurrentCulture = this.currentCulture;
            Thread.CurrentThread.CurrentUICulture = this.currentUICulture;
        }

        [TestMethod]
        public void Should_Return_String_For_en_US()
        {
            CultureInfo englishUs = new CultureInfo("en-US");

            Thread.CurrentThread.CurrentCulture = englishUs;
            Thread.CurrentThread.CurrentUICulture = englishUs;

            IResourceManager resourceManager = new ResxFilesResourceManager(typeof(Labels));

            string key = "INFRASTRUCTURE.TRANSLATEME";

            string actual = resourceManager[key];

            Assert.AreEqual("english", actual);
        }

        [TestMethod]
        public void Should_Return_String_For_de_DE()
        {
            CultureInfo englishUs = new CultureInfo("de-DE");

            Thread.CurrentThread.CurrentCulture = englishUs;
            Thread.CurrentThread.CurrentUICulture = englishUs;

            IResourceManager resourceManager = new ResxFilesResourceManager(typeof(Labels));

            string key = "INFRASTRUCTURE.TRANSLATEME";

            string actual = resourceManager[key];

            Assert.AreEqual("deutsch", actual);
        }
    }
}
