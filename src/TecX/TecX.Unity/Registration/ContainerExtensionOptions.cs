using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public class ContainerExtensionOptions
    {
        public Action<Type, IUnityContainer> Registrator { get; private set; }

        public ContainerExtensionOptions(Action<Type, IUnityContainer> registrator)
        {
            Guard.AssertNotNull(registrator, "registrator");

            Registrator = registrator;
        }
    }
}