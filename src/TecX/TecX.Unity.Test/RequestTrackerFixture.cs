namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Unity.Test.TestObjects;
    using TecX.Unity.Tracking;

    using Xunit;

    public class RequestTrackerFixture
    {
        [Fact]
        public void CanIdentifyParameterResolutionByBuildOperation()
        {
            var container = new UnityContainer();
            container.AddNewExtension<RequestTracker>();

            container.RegisterType<IFoo, Foo>();
            container.RegisterType<IBar, Bar2>();

            Consumer2 consumer = container.Resolve<Consumer2>();

            consumer = container.Resolve<Consumer2>();
        }
    }

    public class Consumer2
    {
        public IFoo Foo { get; set; }

        public IBar Bar { get; set; }

        public Consumer2(IFoo foo, IBar bar)
        {
            this.Foo = foo;
            this.Bar = bar;
        }
    }
}
