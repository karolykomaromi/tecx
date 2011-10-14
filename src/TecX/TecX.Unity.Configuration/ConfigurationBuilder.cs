using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Unity.Configuration.Conventions;
using TecX.Unity.Configuration.Expressions;

namespace TecX.Unity.Configuration
{
    public class ConfigurationBuilder : UnityContainerExtension
    {
        private readonly List<Action<Configuration>> _alternations;

        public ConfigurationBuilder()
        {
            _alternations = new List<Action<Configuration>>();
        }

        public static bool IsPublicBuilder(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type.Assembly == typeof(ConfigurationBuilder).Assembly)
            {
                return false;
            }

            if (!typeof(ConfigurationBuilder).IsAssignableFrom(type))
            {
                return false;
            }

            if (type.IsInterface ||
                type.IsAbstract ||
                type.IsGenericType)
            {
                return false;
            }

            return type.GetConstructor(new Type[0]) != null;
        }

        public void AddType(Type from, Type to, string name)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotEmpty(name, "name");

            this._alternations.Add(config =>
                {
                    var family = config.FindFamily(from);

                    var registration = new TypeRegistration(
                        from,
                        to,
                        name,
                        new TransientLifetimeManager(),
                        new InjectionMember[0]);

                    family.AddRegistration(registration);
                });
        }

        public CreateRegistrationFamilyExpression For<T>()
        {
            return this.For(typeof(T));
        }

        public CreateRegistrationFamilyExpression For(Type from)
        {
            Guard.AssertNotNull(from, "from");

            return new CreateRegistrationFamilyExpression(from, this);
        }

        public void ImportBuilder<T>()
            where T : ConfigurationBuilder, new()
        {
            ImportBuilder(new T());
        }

        public void ImportBuilder(ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(builder, "ConfigurationBuilder");

            _alternations.Add(builder.BuildUp);
        }

        public void Scan(Action<IAssemblyScanner> action)
        {
            Guard.AssertNotNull(action, "action");

            var scanner = new AssemblyScanner();

            action(scanner);

            _alternations.Add(graph => graph.AddScanner(scanner));
        }

        #region Infrastructure

        internal void BuildUp(Configuration config)
        {
            Guard.AssertNotNull(config, "config");

            if (config.Builders.Contains(this))
            {
                return;
            }

            _alternations.ForEach(action => action(config));

            config.Builders.Add(this);
        }

        protected internal void AddExpression(Action<Configuration> alteration)
        {
            Guard.AssertNotNull(alteration, "alteration");

            _alternations.Add(alteration);
        }

        protected sealed override void Initialize()
        {
            PreBuildUp();

            Configuration graph = new Configuration();

            BuildUp(graph);

            graph.Configure(Container);

            PostBuildUp();
        }

        protected virtual void PreBuildUp()
        {
        }

        protected virtual void PostBuildUp()
        {
        }

        #endregion Infrastructure
    }
}
