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
        private readonly List<Action<Configuration>> _actions;
        private readonly List<Action> _basicActions;

        public ConfigurationBuilder()
        {
            _actions = new List<Action<Configuration>>();
            _basicActions = new List<Action>();
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

            return (type.GetConstructor(new Type[0]) != null);
        }

        public void AddType(Type from, Type to, string name)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotEmpty(name, "name");

            _actions.Add(graph =>
            {
                var family = graph.FindFamily(from);

                var registration = new TypeRegistration(from,
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

        public void Import<T>() 
            where T : ConfigurationBuilder, new()
        {
            Import(new T());
        }

        public void Import(ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(builder, "ConfigurationBuilder");

            _actions.Add(builder.BuildUp);
        }

        public void Scan(Action<IAssemblyScanner> action)
        {
            Guard.AssertNotNull(action, "action");

            var scanner = new AssemblyScanner();

            action(scanner);

            _actions.Add(graph => graph.AddScanner(scanner));
        }

        #region Infrastructure

        internal void BuildUp(Configuration config)
        {
            Guard.AssertNotNull(config, "config");

            if (config.Builders.Contains(this))
            {
                return;
            }

            _actions.ForEach(action => action(config));

            config.Builders.Add(this);
        }

        internal void AddExpression(Action<Configuration> alteration)
        {
            Guard.AssertNotNull(alteration, "alteration");

            _actions.Add(alteration);
        }
        
        protected void AddAction(Action action)
        {
            Guard.AssertNotNull(action, "action");

            _basicActions.Add(action);
        }

        protected sealed override void Initialize()
        {
            this.PreBuildUp();

            _basicActions.ForEach(action => action());

            Configuration graph = new Configuration();

            this.BuildUp(graph);

            graph.Configure(Container);

            this.PostBuildUp();
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
