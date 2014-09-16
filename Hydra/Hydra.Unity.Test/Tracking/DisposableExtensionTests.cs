namespace Hydra.Unity.Test.Tracking
{
    using Hydra.Unity.Tracking;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class DisposableExtensionTests
    {
        [Fact]
        public void Should_Dispose_On_TearDown()
        {
            var container = new UnityContainer()
                .AddNewExtension<DisposableExtension>();

            var sut = container.Resolve<DisposeThis>();

            var sut1 = container.Resolve<DisposeThis>();

            container.Teardown(sut);

            Assert.True(sut.IsDisposed);
            Assert.False(sut1.IsDisposed);
        }

        [Fact]
        public void Should_Dispose_All_Tree_On_Container_Dispose()
        {
            DisposeThis sut1, sut2;

            using (IUnityContainer container = new UnityContainer()
                .AddNewExtension<DisposableExtension>())
            {
                sut1 = container.Resolve<DisposeThis>();

                sut2 = container.Resolve<DisposeThis>();
            }

            Assert.True(sut1.IsDisposed);
            Assert.True(sut2.IsDisposed);
        }

        [Fact]
        public void Should_Not_Dispose_Item_With_ContainerControlledLifetime_On_Teardown()
        {
            var container = new UnityContainer()
                .AddNewExtension<DisposableExtension>()
                .RegisterType<WithSingletonLifetime>(new ContainerControlledLifetimeManager());

            var sut = container.Resolve<WithSingletonLifetime>();

            container.Teardown(sut);

            Assert.False(sut.IsDisposed);
        }

        [Fact]
        public void Should_Not_Dispose_Item_With_HierarchicalLifetime_On_Teardown()
        {
            var container = new UnityContainer()
                .AddNewExtension<DisposableExtension>()
                .RegisterType<WithSingletonLifetime>(new HierarchicalLifetimeManager());

            var sut = container.Resolve<WithSingletonLifetime>();

            container.Teardown(sut);

            Assert.False(sut.IsDisposed);
        }

        [Fact]
        public void Should_Not_Dispose_Objects_Not_Created_By_Container()
        {
            NotCreatedByContainer ncc = new NotCreatedByContainer();

            TakesItemNotCreatedByContainer t;

            using (var container = new UnityContainer().AddNewExtension<DisposableExtension>().RegisterType<TakesItemNotCreatedByContainer>(new InjectionConstructor(ncc)))
            {
                t = container.Resolve<TakesItemNotCreatedByContainer>();
            }

            Assert.True(t.IsDisposed);
            Assert.False(ncc.IsDisposed);
            Assert.Same(ncc, t.OutsideReference);
        }

        [Fact]
        public void Should_Dispose_Objects_Resolved_From_Child_Container_When_Child_Container_Is_Disposed()
        {
            var container = new UnityContainer().AddNewExtension<DisposableExtension>();

            DisposeThis fromParent = container.Resolve<DisposeThis>();

            DisposeThis fromChild;

            // TODO weberse 2014-09-16 should work if child container ran in separate thread...
            using (var childContainer = container.CreateChildContainer().AddNewExtension<DisposableExtension>())
            {
                fromChild = childContainer.Resolve<DisposeThis>();
            }

            Assert.False(fromParent.IsDisposed);
            Assert.True(fromChild.IsDisposed);
        }
    }
}