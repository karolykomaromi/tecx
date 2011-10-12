using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Unity.Configuration.Conventions;
using TecX.Unity.Configuration.Expressions;

namespace TecX.Unity.Configuration
{
    public class Registry : UnityContainerExtension
    {
        private readonly List<Action<RegistrationGraph>> _actions;
        private readonly List<Action> _basicActions;

        public Registry()
        {
            _actions = new List<Action<RegistrationGraph>>();
            _basicActions = new List<Action>();
        }

        public static bool IsPublicRegistry(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type.Assembly == typeof(Registry).Assembly)
            {
                return false;
            }

            if (!typeof(Registry).IsAssignableFrom(type))
            {
                return false;
            }

            if (type.IsInterface || type.IsAbstract || type.IsGenericType)
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

        public void IncludeRegistry<T>() 
            where T : Registry, new()
        {
            _actions.Add(g => new T().ConfigureRegistrationGraph(g));
        }

        public void IncludeRegistry(Registry registry)
        {
            Guard.AssertNotNull(registry, "registry");

            _actions.Add(registry.ConfigureRegistrationGraph);
        }

        public void Scan(Action<IAssemblyScanner> action)
        {
            Guard.AssertNotNull(action, "action");

            var scanner = new AssemblyScanner();

            action(scanner);

            _actions.Add(graph => graph.AddScanner(scanner));
        }

        #region Infrastructure

        internal void ConfigureRegistrationGraph(RegistrationGraph graph)
        {
            Guard.AssertNotNull(graph, "graph");

            if (graph.Registries.Contains(this))
            {
                return;
            }

            _actions.ForEach(action => action(graph));

            graph.Registries.Add(this);
        }

        internal void AddExpression(Action<RegistrationGraph> alteration)
        {
            Guard.AssertNotNull(alteration, "alteration");

            _actions.Add(alteration);
        }
        
        protected void AddAction(Action action)
        {
            Guard.AssertNotNull(action, "action");

            _basicActions.Add(action);
        }

        protected override void Initialize()
        {
            _basicActions.ForEach(action => action());

            RegistrationGraph graph = new RegistrationGraph();

            ConfigureRegistrationGraph(graph);

            graph.Configure(Container);
        }

        #endregion Infrastructure
    }
}
