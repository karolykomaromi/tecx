namespace TecX.Unity.Test.Factories
{
    using System;
    using Microsoft.Practices.Unity;
    using TecX.Unity.Factories;
    using Xunit;

    public class DelegateFactoryFixture
    {
        [Fact]
        public void Should_Resolve_Delegate()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IRepository, Repository>();
            container.RegisterType<SomeFactoryDelegate>(new DelegateFactory());

            SomeFactoryDelegate factory = container.Resolve<SomeFactoryDelegate>();

            IRepository repository = factory(true);

            Assert.NotNull(repository);
            Assert.True(repository.IsReadOnly);
        }

        [Fact]
        public void Should_Construct_Delegate()
        {
            var container = new UnityContainer();

            container.RegisterType<IRepository, Repository>();

            Type delegateType = typeof(SomeFactoryDelegate);

            Delegate factory = DelegateFactoryBuildPlanPolicy.GetDelegate(container, delegateType);

            IRepository repository = factory.DynamicInvoke(true) as IRepository;

            Assert.NotNull(repository);
            Assert.True(repository.IsReadOnly);
        }

        [Fact]
        public void Should_Resolve_Func_Factory_Delegate()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IRepository, Repository>();
            container.RegisterType<Func<bool, IRepository>>(new DelegateFactory());

            Func<bool, IRepository> factory = container.Resolve<Func<bool, IRepository>>();

            IRepository repository = factory(true);

            Assert.NotNull(repository);
            Assert.True(repository.IsReadOnly);
        }

        [Fact]
        public void Should_Use_Overrides_In_Order_Of_Appearance_When_Resolving_Func()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<Func<string, string, string, OrderOfAppearanceMatters>>(new DelegateFactory());

            var factory = container.Resolve<Func<string, string, string, OrderOfAppearanceMatters>>();

            var sut = factory("1", "2", "3");

            Assert.Equal("1", sut.First);
            Assert.Equal("2", sut.Second);
            Assert.Equal("3", sut.Third);
        }
    }

    public class OrderOfAppearanceMatters
    {
        public OrderOfAppearanceMatters(string first, string second, string third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }

        public string First { get; private set; }

        public string Second { get; private set; }

        public string Third { get; private set; }
    }

    public delegate IRepository SomeFactoryDelegate(bool isReadOnly);

    public class Repository : IRepository
    {
        public Repository(bool isReadOnly)
        {
            this.IsReadOnly = isReadOnly;
        }

        public bool IsReadOnly { get; private set; }
    }

    public interface IRepository
    {
        bool IsReadOnly { get; }
    }
}
