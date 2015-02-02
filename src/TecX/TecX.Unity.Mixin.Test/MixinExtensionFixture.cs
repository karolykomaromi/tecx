namespace TecX.Unity.Mixin.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Remotion.Mixins;

    using TecX.Unity.Mixin.Test.TestObjects;

    [TestClass]
    public class MixinExtensionFixture
    {
        [TestMethod]
        public void CanResolveObjectsWithMixinsViaContainer()
        {
            var builder = MixinConfiguration.BuildNew();

            builder.ForClass<MixinTarget>().AddMixin<ToStringMixin>();

            var config = builder.BuildConfiguration();

            var container = new UnityContainer();

            container.AddNewExtension<MixinExtension>();

            container.RegisterType<ILogger, Logger>();

            container.RegisterType<MixinTarget>();

            using (config.EnterScope())
            {
                var target = container.Resolve<MixinTarget>();

                Assert.AreEqual("2", target.ToString());
            }
        }
    }
}
