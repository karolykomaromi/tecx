namespace Hydra.Unity.Test.Tracking
{
    using Hydra.Unity.Tracking;
    using Microsoft.Practices.Unity;
    using Xunit;
    using Xunit.Extensions;

    public class DisposableExtensionTests
    {
        [Theory, ContainerData]
        public void Should_Dispose_On_TearDown(IUnityContainer container)
        {
            var sut = container.Resolve<DisposeThis>();

            var sut1 = container.Resolve<DisposeThis>();

            container.Teardown(sut);

            Assert.True(sut.IsDisposed);
            Assert.False(sut1.IsDisposed);
        }

        [Theory, ContainerData]
        public void Should_Dispose_All_Trees_On_Container_Dispose(IUnityContainer container)
        {
            DisposeThis sut1, sut2;

            using (container)
            {
                sut1 = container.Resolve<DisposeThis>();

                sut2 = container.Resolve<DisposeThis>();
            }

            Assert.True(sut1.IsDisposed);
            Assert.True(sut2.IsDisposed);
        }

        [Theory, ContainerData]
        public void Should_Not_Dispose_Item_With_ContainerControlledLifetime_On_Teardown(IUnityContainer container)
        {
            container.RegisterType<WithSingletonLifetime>(new ContainerControlledLifetimeManager());

            var sut = container.Resolve<WithSingletonLifetime>();

            container.Teardown(sut);

            Assert.False(sut.IsDisposed);
        }

        [Theory, ContainerData]
        public void Should_Not_Dispose_Item_With_HierarchicalLifetime_On_Teardown(IUnityContainer container)
        {
            container.RegisterType<WithSingletonLifetime>(new HierarchicalLifetimeManager());

            var sut = container.Resolve<WithSingletonLifetime>();

            container.Teardown(sut);

            Assert.False(sut.IsDisposed);
        }

        [Theory, ContainerData]
        public void Should_Not_Dispose_Objects_Not_Created_By_Container(IUnityContainer container)
        {
            NotCreatedByContainer ncc = new NotCreatedByContainer();

            TakesItemNotCreatedByContainer t;

            using (container.RegisterType<TakesItemNotCreatedByContainer>(new InjectionConstructor(ncc)))
            {
                t = container.Resolve<TakesItemNotCreatedByContainer>();
            }

            Assert.True(t.IsDisposed);
            Assert.False(ncc.IsDisposed);
            Assert.Same(ncc, t.OutsideReference);
        }

        [Theory, ContainerData]
        public void Should_Dispose_Objects_Resolved_From_Child_Container_When_Child_Container_Is_Disposed(IUnityContainer container)
        {
            DisposeThis fromParent, fromChild1;

            using (var child1 = container.CreateChildContainer())
            {
                fromParent = container.Resolve<DisposeThis>();

                DisposeThis fromChild2;

                using (var child2 = child1.CreateChildContainer())
                {
                    fromChild2 = child2.Resolve<DisposeThis>();
                }

                Assert.False(fromParent.IsDisposed);
                Assert.True(fromChild2.IsDisposed);

                Assert.Equal(1, container.Configure<DisposableExtension>().Tracker.BuildTrees.Count);

                fromChild1 = child1.Resolve<DisposeThis>();
            }

            Assert.False(fromParent.IsDisposed);
            Assert.True(fromChild1.IsDisposed);
            Assert.Equal(1, container.Configure<DisposableExtension>().Tracker.BuildTrees.Count);
        }

        [Theory, ContainerData]
        public void Should_Dispose_Objects_In_Injection_Hierarchy_When_Levels_In_Between_Are_Not_Disposable(IUnityContainer container)
        {
            Level0 level0;
            using (container)
            {
                level0 = container.Resolve<Level0>();
            }

            Assert.True(level0.IsDisposed);
            Assert.True(level0.Level1.Level2.Level3.IsDisposed);
        }
    }
}