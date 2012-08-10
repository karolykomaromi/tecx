namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Unity.Test.TestObjects;
    using TecX.Unity.ContextualBinding;
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

        [Fact]
        public void CanResolveLogWithParentType()
        {
            var container = new UnityContainer();

            container.AddNewExtension<RequestTracker>();

            container.RegisterType<ILog>(
                new InjectionFactory((ctr, type, name) =>
                {
                    var parentType = Request.Current.ParentRequest.BuildKey.Type;

                    return LogManager.GetLogger(parentType);
                }));

            var sut = container.Resolve<UsesLog>();

            // InjectionFactory resolves IUnityContainer which is not a target available for UsesLog.
            Assert.Equal(typeof(UsesLog), sut.Log.Type);
        }

        [Fact]
        public void BuildTreeNodeOnCorrectLevel()
        {
            var container = new UnityContainer();

            container.RegisterType<IFoo, Foo>(
                request => request != null);

            IFoo foo = container.Resolve<IFoo>();

            Assert.NotNull(foo);
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
