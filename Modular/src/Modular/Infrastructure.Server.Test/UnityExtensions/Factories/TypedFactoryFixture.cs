namespace Infrastructure.Server.Test.UnityExtensions.Factories
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.UnityExtensions.Factories;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Xunit;

    public class TypedFactoryFixture
    {
        [Fact]
        public void Should_Resolve_Generated_Factory()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IGeneratedFactory>(new TypedFactory());

            IGeneratedFactory factory = container.Resolve<IGeneratedFactory>();

            Assert.NotNull(factory);
        }

        [Fact]
        public void Should_Create_Object_From_Generated_Factory()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();

            container.RegisterType<ITypedFactoryTestObject, P1>();

            container.RegisterType<IGeneratedFactory>(new TypedFactory());

            IGeneratedFactory factory = container.Resolve<IGeneratedFactory>();

            ITypedFactoryTestObject obj = factory.Create();

            Assert.NotNull(obj);
        }

        [Fact]
        public void Should_Create_Object_With_Specified_Parameter_From_Generated_Factory()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();

            container.RegisterType<ITypedFactoryTestObject, P3>();

            container.RegisterType<IGeneratedFactory>(new TypedFactory());

            IGeneratedFactory factory = container.Resolve<IGeneratedFactory>();

            ITypedFactoryTestObject obj = factory.Create("someRatherLongName");

            Assert.Equal("someRatherLongName", obj.Name);
        }

        [Fact]
        public void Should_Create_Array_From_Generated_Factory()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();

            container.RegisterType<ITypedFactoryTestObject, P1>("1");
            container.RegisterType<ITypedFactoryTestObject, P2>("2");

            container.RegisterType<IGeneratedFactory>(new TypedFactory());

            IGeneratedFactory factory = container.Resolve<IGeneratedFactory>();

            ITypedFactoryTestObject[] array = factory.CreateArray();

            Assert.Equal(2, array.Length);
        }

        [Fact]
        public void Should_Create_IEnumerable_From_Generated_Factory()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();

            container.RegisterType<ITypedFactoryTestObject, P1>("1");
            container.RegisterType<ITypedFactoryTestObject, P2>("2");

            container.RegisterType<IGeneratedFactory>(new TypedFactory());

            IGeneratedFactory factory = container.Resolve<IGeneratedFactory>();

            IEnumerable<ITypedFactoryTestObject> enumerable = factory.CreateEnumerable();

            Assert.Equal(2, enumerable.Count());
        }

        [Fact]
        public void Should_Create_ICollection_From_Generated_Factory()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();

            container.RegisterType<ITypedFactoryTestObject, P1>("1");
            container.RegisterType<ITypedFactoryTestObject, P2>("2");

            container.RegisterType<IGeneratedFactory>(new TypedFactory());

            IGeneratedFactory factory = container.Resolve<IGeneratedFactory>();

            ICollection<ITypedFactoryTestObject> collection = factory.CreateCollection();

            Assert.Equal(2, collection.Count);
        }

        [Fact]
        public void Should_Create_Collection_With_Specified_Parameter_From_Generated_Factory()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();

            container.RegisterType<ITypedFactoryTestObject, P1>("1");
            container.RegisterType<ITypedFactoryTestObject, P2>("2");
            container.RegisterType<ITypedFactoryTestObject, P3>("3");

            container.RegisterType<IGeneratedFactory>(new TypedFactory());

            IGeneratedFactory factory = container.Resolve<IGeneratedFactory>();

            ICollection<ITypedFactoryTestObject> collection = factory.CreateCollection("someRatherLongName");

            Assert.Equal(3, collection.Count);

            P3 p3 = collection.OfType<P3>().Single();

            Assert.Equal("someRatherLongName", p3.Name);
        }

        [Fact]
        public void Should_Create_IList_From_Generated_Factory()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();

            container.RegisterType<ITypedFactoryTestObject, P1>("1");
            container.RegisterType<ITypedFactoryTestObject, P2>("2");

            container.RegisterType<IGeneratedFactory>(new TypedFactory());

            IGeneratedFactory factory = container.Resolve<IGeneratedFactory>();

            IList<ITypedFactoryTestObject> list = factory.CreateList();

            Assert.Equal(2, list.Count);
        }
    }

    public interface IGeneratedFactory
    {
        ITypedFactoryTestObject Create();

        ITypedFactoryTestObject Create(string name);

        ITypedFactoryTestObject[] CreateArray();

        IEnumerable<ITypedFactoryTestObject> CreateEnumerable();

        IList<ITypedFactoryTestObject> CreateList();

        ICollection<ITypedFactoryTestObject> CreateCollection();

        ICollection<ITypedFactoryTestObject> CreateCollection(string name);
    }

    public class P3 : ITypedFactoryTestObject
    {
        public P3(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }

    public class P2 : ITypedFactoryTestObject
    {
        public string Name { get; private set; }
    }

    public interface ITypedFactoryTestObject
    {
        string Name { get; }
    }

    public class P1 : ITypedFactoryTestObject
    {
        public string Name { get; set; }
    }
}
