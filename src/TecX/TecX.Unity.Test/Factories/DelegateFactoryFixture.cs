namespace TecX.Unity.Test.Factories
{
    using System;
    using Microsoft.Practices.Unity;
    using TecX.Unity.Factories;
    using TecX.Unity.Test.TestObjects;
    using Xunit;

    public class DelegateFactoryFixture
    {
        [Fact]
        public void CanCreateDelegate()
        {
            var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<UnitOfWorkFactory>(new DelegateFactory());

            var consumer = container.Resolve<Consumer>();

            var uow = consumer.Factory(true) as UnitOfWork;

            Assert.NotNull(uow);
            Assert.True(uow.ReadOnly);
        }

        [Fact]
        public void Test()
        {
            var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            Type delegateType = typeof(UnitOfWorkFactory);

            Delegate @delegate = DelegateFactoryBuildPlanPolicy.GetDelegate(container, delegateType);

            UnitOfWork uow = @delegate.DynamicInvoke(true) as UnitOfWork;

            Assert.NotNull(uow);
            Assert.True(uow.ReadOnly);
        }

        [Fact]
        public void Blueprint()
        {
            var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            Func<bool, IUnitOfWork> func = readOnly =>
            {
                ParameterOverride po1 = new ParameterOverride("readOnly", readOnly);

                return container.Resolve<IUnitOfWork>(po1);
            };

            UnitOfWorkFactory factory = new UnitOfWorkFactory(func);

            UnitOfWork uow = factory(true) as UnitOfWork;

            Assert.NotNull(uow);
            Assert.True(uow.ReadOnly);
        }

        [Fact]
        public void TheRealThing()
        {
            var container = new UnityContainer();

            container.RegisterType<UnitOfWorkFactory>(new DelegateFactory());

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            Consumer consumer = container.Resolve<Consumer>();

            UnitOfWork uow = consumer.Factory(true) as UnitOfWork;

            Assert.NotNull(uow);
            Assert.True(uow.ReadOnly);
        }
    }

    public delegate IUnitOfWork UnitOfWorkFactory(bool readOnly);

    public class Consumer
    {
        public Consumer(UnitOfWorkFactory factory)
        {
            this.Factory = factory;
        }

        public UnitOfWorkFactory Factory { get; set; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(bool readOnly)
        {
            this.ReadOnly = readOnly;
        }

        public bool ReadOnly { get; set; }
    }
}
