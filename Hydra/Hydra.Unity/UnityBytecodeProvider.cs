namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using NHibernate.Bytecode;
    using NHibernate.Bytecode.Lightweight;
    using NHibernate.Properties;

    public class UnityBytecodeProvider : IBytecodeProvider
    {
        private readonly IDictionary<Type, bool> isRegistered;
        private readonly IUnityContainer container;
        private readonly IProxyFactoryFactory proxyFactoryFactory;
        private readonly ICollectionTypeFactory collectionTypeFactory;
        private readonly IObjectsFactory objectsFactory;

        public UnityBytecodeProvider(IUnityContainer container, IProxyFactoryFactory proxyFactoryFactory, ICollectionTypeFactory collectionTypeFactory)
        {
            this.container = container;
            this.proxyFactoryFactory = proxyFactoryFactory;
            this.collectionTypeFactory = collectionTypeFactory;
            this.objectsFactory = new UnityObjectsFactory(container);

            this.isRegistered = new Dictionary<Type, bool>();
        }

        public IProxyFactoryFactory ProxyFactoryFactory
        {
            get { return this.proxyFactoryFactory; }
        }

        public IObjectsFactory ObjectsFactory
        {
            get { return this.objectsFactory; }
        }

        public ICollectionTypeFactory CollectionTypeFactory
        {
            get { return this.collectionTypeFactory; }
        }

        public IReflectionOptimizer GetReflectionOptimizer(Type clazz, IGetter[] getters, ISetter[] setters)
        {
            bool registered;
            if (!this.isRegistered.TryGetValue(clazz, out registered))
            {
                if (clazz.IsInterface || clazz.IsAbstract)
                {
                    var registrations = this.container.Registrations
                                            .Where(r => r.RegisteredType == clazz)
                                            .OrderBy(r => r.Name, StringComparer.OrdinalIgnoreCase)
                                            .ToArray();

                    if (registrations.Length > 0)
                    {
                        registered = true;
                        this.isRegistered[clazz] = true;
                    }
                    else
                    {
                        this.isRegistered[clazz] = false;
                    }
                }
                else
                {
                    registered = true;
                    this.isRegistered[clazz] = true;
                }
            }

            if (registered)
            {
                return new UnityReflectionOptimizer(this.container, clazz, getters, setters);
            }

            return new ReflectionOptimizer(clazz, getters, setters);
        }
    }
}