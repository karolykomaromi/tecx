using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Conventions;
using TecX.Unity.Configuration.Test.TestObjects;

namespace TecX.Unity.Configuration.Test
{
    [TestClass]
    public class When_ApplyingFindConfigBuildersConvention : Given_ContainerAndBuilder
    {
        protected override void Given()
        {
            base.Given();

            builder.Scan(
                s =>
                {
                    s.With(new FindConfigBuildersConvention());

                    s.AssemblyContainingType(typeof(ConfigurationBuilderSubClass));
                });
        }

        [TestMethod]
        public void Then_FindsConfigBuilders()
        {
            IRepository<int> repository = container.Resolve<IRepository<int>>();

            Assert.IsNotNull(repository);
        }
    }
}
