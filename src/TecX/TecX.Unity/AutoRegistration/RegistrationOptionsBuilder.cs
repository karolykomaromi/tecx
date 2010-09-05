using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    public class RegistrationOptionsBuilder
    {
        #region Fields

        private Type _type;

        private readonly List<Func<Type, InjectionMember>> _injectionMembers = new List<Func<Type, InjectionMember>>();

        private Func<Type, IEnumerable<Type>> _interfacesToRegisterAsResolver = t => new List<Type>(t.GetInterfaces());
        private Func<Type, string> _nameToRegisterWithResolver = t => String.Empty;
        private Func<Type, LifetimeManager> _lifetimeManagerToRegisterWithResolver = t => new TransientLifetimeManager();

        #endregion Fields

        #region Lifetime

        /// <summary>
        /// Specifies lifetime manager to use when registering type
        /// </summary>
        /// <typeparam name="TLifetimeManager">The type of the lifetime manager.</typeparam>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder UsingLifetime<TLifetimeManager>()
            where TLifetimeManager : LifetimeManager, new()
        {
            _lifetimeManagerToRegisterWithResolver = t => new TLifetimeManager();

            return this;
        }

        /// <summary>
        /// Specifies lifetime manager resolver function, that by given type return lifetime manager to use when registering type
        /// </summary>
        /// <param name="lifetimeResolver">Lifetime manager resolver.</param>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder UsingLifetime(Func<Type, LifetimeManager> lifetimeResolver)
        {
            Guard.AssertNotNull(lifetimeResolver, "lifetimeResolver");

            _lifetimeManagerToRegisterWithResolver = lifetimeResolver;

            return this;
        }

        /// <summary>
        /// Specifies ContainerControlledLifetimeManager lifetime manager to use when registering type
        /// </summary>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder UsingSingletonMode()
        {
            _lifetimeManagerToRegisterWithResolver = t => new ContainerControlledLifetimeManager();

            return this;
        }

        /// <summary>
        /// Specifies TransientLifetimeManager lifetime manager to use when registering type
        /// </summary>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder UsingPerCallMode()
        {
            _lifetimeManagerToRegisterWithResolver = t => new TransientLifetimeManager();

            return this;
        }

        /// <summary>
        /// Specifies PerThreadLifetimeManager lifetime manager to use when registering type
        /// </summary>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder UsingPerThreadMode()
        {
            _lifetimeManagerToRegisterWithResolver = t => new PerThreadLifetimeManager();

            return this;
        }


        #endregion Lifetime

        #region Named Registration

        /// <summary>
        /// Specifies name to register type with
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder WithName(string name)
        {
            Guard.AssertNotNull(name, "name");

            _nameToRegisterWithResolver = t => name;

            return this;
        }

        /// <summary>
        /// Specifies name resolver function that by given type returns name to register it with
        /// </summary>
        /// <param name="nameResolver">Name resolver.</param>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder WithName(Func<Type, string> nameResolver)
        {
            Guard.AssertNotNull(nameResolver, "nameResolver");

            _nameToRegisterWithResolver = nameResolver;

            return this;
        }

        /// <summary>
        /// Specifies that type name should be used to register it with
        /// </summary>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder WithTypeName()
        {
            _nameToRegisterWithResolver = t => t.Name;

            return this;
        }

        /// <summary>
        /// Specifies that type should be registered with its name minus well-known application part name.
        /// For example: WithPartName("Controller") will register 'HomeController' type with name 'Home',
        /// or WithPartName(WellKnownAppParts.Repository) will register 'CustomerRepository' type with name 'Customer'
        /// </summary>
        /// <param name="name">Application part name.</param>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder WithPartName(string name)
        {
            Guard.AssertNotNull(name, "name");

            _nameToRegisterWithResolver = t =>
                                              {
                                                  var typeName = t.Name;
                                                  if (typeName.EndsWith(name))
                                                      return typeName.Remove(typeName.Length - name.Length);
                                                  return typeName;
                                              };
            return this;
        }


        #endregion Named Registration

        #region Abstraction

        /// <summary>
        /// Specifies interface to register type as
        /// </summary>
        /// <typeparam name="TContract">The type of the interface.</typeparam>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder As<TContract>() where TContract : class
        {
            _interfacesToRegisterAsResolver = t => new List<Type> { typeof(TContract) };
            return this;
        }

        /// <summary>
        /// Specifies interface resolver function that by given type returns interface register type as
        /// </summary>
        /// <param name="typeResolver">Interface resolver.</param>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder As(Func<Type, Type> typeResolver)
        {
            Guard.AssertNotNull(typeResolver, "typeResolver");

            _interfacesToRegisterAsResolver = t => new List<Type> { typeResolver(t) };
            return this;
        }

        /// <summary>
        /// Specifies interface resolver function that by given type returns interfaces register type as
        /// </summary>
        /// <param name="typesResolver">Interface resolver.</param>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder As(Func<Type, Type[]> typesResolver)
        {
            Guard.AssertNotNull(typesResolver, "typesResolver");

            _interfacesToRegisterAsResolver = t => new List<Type>(typesResolver(t));
            return this;
        }

        /// <summary>
        /// Specifies that type should be registered as its first interface
        /// </summary>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder AsFirstInterfaceOfType()
        {
            _interfacesToRegisterAsResolver = t => new List<Type> { t.GetInterfaces().First() };
            return this;
        }

        /// <summary>
        /// Specifies that type should be registered as its single interface
        /// </summary>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder AsSingleInterfaceOfType()
        {
            _interfacesToRegisterAsResolver = t => new List<Type> { t.GetInterfaces().Single() };
            return this;
        }

        /// <summary>
        /// Specifies that type should be registered as all its interfaces
        /// </summary>
        /// <returns>Fluent registration</returns>
        public RegistrationOptionsBuilder AsAllInterfacesOfType()
        {
            _interfacesToRegisterAsResolver = t => new List<Type>(t.GetInterfaces());
            return this;
        }


        #endregion Abstraction

        #region c'tor Arguments

        public RegistrationOptionsBuilder WithCtorArg(string name, object value)
        {
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotNull(value, "value");

            _injectionMembers.Add(t => new ClozeInjectionConstructur(name, value));

            return this;
        }

        #endregion c'tor Arguments
        
        public RegistrationOptionsBuilder ForType(Type type)
        {
            Guard.AssertNotNull(type, "type");

            _type = type;

            return this;
        }

        public IEnumerable<RegistrationOptions> Build()
        {
            if (_type == null) throw new InvalidOperationException("Must set the type which should be the mapping target");

            List<RegistrationOptions> registrations = new List<RegistrationOptions>();

            foreach (Type abstraction in _interfacesToRegisterAsResolver(_type))
            {
                registrations.Add(
                    new RegistrationOptions(abstraction, 
                                            _type, 
                                            _nameToRegisterWithResolver(_type), _lifetimeManagerToRegisterWithResolver(_type)));

            }

            return registrations;
        }
    }
}
