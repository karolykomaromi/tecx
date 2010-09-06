using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;

using UnityAutoRegistration;

namespace TecX.Unity
{
    public class RegistrationOptionsBuilder
    {
        private Type _type;

        private Func<Type, IEnumerable<Type>> _abstractions = t => new List<Type>(t.GetInterfaces());
        private Func<Type, string> _name = t => string.Empty;
        private Func<Type, LifetimeManager> _lifetimeManager = t => new TransientLifetimeManager();
        private Func<Type, InjectionMember[]> _injectionMember = t => null;


        public RegistrationOptionsBuilder WithName(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            _name = t => name;

            return this;
        }

        public IEnumerable<RegistrationOptions> Build()
        {
            List<RegistrationOptions> options = new List<RegistrationOptions>();

            foreach (Type from in _abstractions(_type))
            {
                RegistrationOptions option = new RegistrationOptions(
                    from, 
                    _type, 
                    _name(_type), 
                    _lifetimeManager(_type),
                    _injectionMember(_type));

                options.Add(option);
            }

            return options;
        }

        public RegistrationOptionsBuilder MappingTo<TTo>()
            where TTo : class
        {            
            return MappingTo(typeof(TTo));
        }

        public RegistrationOptionsBuilder MappingTo(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            _type = type;

            return this;
        }

        public RegistrationOptionsBuilder WithTypeName()
        {
            _name = t => t.Name;

            return this;
        }

        public RegistrationOptionsBuilder WithIContractName()
        {
            _name = t =>
                            {
                                string contractName = t.GetInterfaces()
                                    .Where(contract => t.Name.EndsWith(contract.Name.Substring(1), StringComparison.OrdinalIgnoreCase))
                                    .FirstOrDefault().Name;

                                return t.Name.Remove(t.Name.Length - contractName.Length + 1);
                            };

            return this;
        }

        public RegistrationOptionsBuilder As<T>()
            where T : class
        {
            _abstractions = t => new[] { typeof(T) };

            return this;
        }

        public RegistrationOptionsBuilder As<T1, T2>()
            where T1 : class
            where T2 : class
        {
            _abstractions = t => new[] 
                                    { 
                                        typeof(T1), 
                                        typeof(T2) 
                                    };

            return this;
        }

        public RegistrationOptionsBuilder As<T1, T2, T3>()
            where T1 : class
            where T2 : class
            where T3 : class
        {
            _abstractions = t => new[] 
                                    { 
                                        typeof(T1), 
                                        typeof(T2), 
                                        typeof(T3)
                                    };

            return this;
        }

        public RegistrationOptionsBuilder As<T1, T2, T3, T4>()
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            _abstractions = t => new[] 
                                    { 
                                        typeof(T1), 
                                        typeof(T2), 
                                        typeof(T3),
                                        typeof(T4)
                                    };

            return this;
        }

        public RegistrationOptionsBuilder As<T1, T2, T3, T4, T5>()
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            _abstractions = t => new[] 
                                    { 
                                        typeof(T1), 
                                        typeof(T2), 
                                        typeof(T3),
                                        typeof(T4),
                                        typeof(T5)
                                    };

            return this;
        }

        public RegistrationOptionsBuilder As<T1, T2, T3, T4, T5, T6>()
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
        {
            _abstractions = t => new[] 
                                    { 
                                        typeof(T1), 
                                        typeof(T2), 
                                        typeof(T3),
                                        typeof(T4),
                                        typeof(T5),
                                        typeof(T6)
                                    };

            return this;
        }

        public RegistrationOptionsBuilder AsSingleInterfaceOfType()
        {
            _abstractions = t => new[] { t.GetInterfaces().Single() };

            return this;
        }

        public RegistrationOptionsBuilder AsAllInterfacesOfType()
        {
            _abstractions = t => t.GetInterfaces();

            return this;
        }

        public RegistrationOptionsBuilder AsFirstInterfaceOfType()
        {
            _abstractions = t => new[] { t.GetInterfaces().First() };

            return this;
        }

        public RegistrationOptionsBuilder UsingPerCallMode()
        {
            _lifetimeManager = t => new TransientLifetimeManager();

            return this;
        }

        public RegistrationOptionsBuilder UsingSingletonMode()
        {
            _lifetimeManager = t => new ContainerControlledLifetimeManager();

            return this;
        }

        public RegistrationOptionsBuilder UsingPerThreadMode()
        {
            _lifetimeManager = t => new PerThreadLifetimeManager();

            return this;
        }

        public RegistrationOptionsBuilder UsingLifetime<TLifetimeManager>()
            where TLifetimeManager : LifetimeManager, new()
        {
            _lifetimeManager = t => new TLifetimeManager();

            return this;
        }

        public RegistrationOptionsBuilder WithoutPartName(string partName)
        {
            if (partName == null) throw new ArgumentNullException("partName");

            _name = t => t.Name.Replace(partName, string.Empty);

            return this;
        }
    }
}
