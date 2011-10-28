using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Remotion.Mixins;

using TecX.Unity.Mixin.Test.TestObjects;

namespace TecX.Unity.Mixin.Test
{
    [TestClass]
    public class MixinExtensionFixture
    {
        [TestMethod]
        public void CanResolveObjectsWithMixinsViaContainer()
        {
            var builder = MixinConfiguration.BuildNew();

            builder.ForClass<MixinTarget>().AddMixin<ToStringMixin>();

            var config = builder.BuildConfiguration();

            using (config.EnterScope())
            {
                var container = new UnityContainer();

                container.AddNewExtension<MixinExtension>();

                container.RegisterType<ILogger, Logger>();

                container.RegisterType<MixinTarget>();

                var target = container.Resolve<MixinTarget>();

                Assert.AreEqual("2", target.ToString());
            }
        }
    }
}
