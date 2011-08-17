using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class TypedFactoryExtension : UnityContainerExtension, ITypedFactoryConfiguration
    {
        protected override void Initialize()
        {

        }

        public void RegisterFactory<TFactory>()
        {
            Type factoryType = typeof(TFactory);

            Guard.AssertCondition(factoryType.IsInterface,
                factoryType,
                "TFactory",
                "Cannot generate an implementation for a non-interface factory type.");

            InterfaceInterceptorClassGenerator generator = new InterfaceInterceptorClassGenerator(typeof(IEmpty), new[] { factoryType });

            Type generatedType = generator.CreateProxyType();

            //var instance = Activator.CreateInstance(generatedType, new Empty(), typeof(IEmpty));

            Container.RegisterType(factoryType, generatedType, new InjectionFactory(container => Activator.CreateInstance(generatedType, new Empty(), typeof(IEmpty))));
        }
    }

    public class Empty : IEmpty
    {
        
    }

    public interface IEmpty
    {
    }
}
