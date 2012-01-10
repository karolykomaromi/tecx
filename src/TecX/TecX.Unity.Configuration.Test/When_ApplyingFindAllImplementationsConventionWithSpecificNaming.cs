namespace TecX.Unity.Configuration.Test
{
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Conventions;
    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_ApplyingFindAllImplementationsConventionWithSpecificNaming : Given_ContainerAndBuilder
    {
        protected override void Given()
        {
            base.Given();

            builder.Scan(s =>
                {
                    var convention = new FindAllImplementationsConvention(typeof(IMyInterface));

                    convention.NameBy(t => t.FullName + "123");

                    s.With(convention);

                    s.AssemblyContainingType<IMyInterface>();

                    s.ExcludeType<MyClassWithCtorParams>();
                });
        }

        [TestMethod]
        public void Then_FindsAllTypesImplementingInterface()
        {
            var results = container.ResolveAll<IMyInterface>();

            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count());
        }
    }
}