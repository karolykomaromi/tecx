namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Conventions;
    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_ApplyingFindConfigBuildersConvention : Given_ContainerAndBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

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
