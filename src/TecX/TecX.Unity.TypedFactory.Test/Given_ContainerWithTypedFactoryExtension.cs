using System.Collections.Generic;

using Microsoft.Practices.Unity;

using TecX.TestTools;
using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    public abstract class Given_ContainerWithTypedFactoryExtension : GivenWhenThen
    {
        protected IUnityContainer container;

        protected IFoo _foo;

        protected IMyFactory _factory;

        protected override void Given()
        {
            container = new UnityContainer();
            container.AddNewExtension<TypedFactoryExtension>();
            container.RegisterFactory<IMyFactory>();

            _factory = container.Resolve<IMyFactory>();
        }
    }
}