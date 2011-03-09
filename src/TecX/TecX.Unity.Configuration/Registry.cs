using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Unity.Configuration.Conventions;
using TecX.Unity.Configuration.Expressions;

namespace TecX.Unity.Configuration
{
    public class Registry
    {
        #region Fields

        private readonly List<Action<RegistrationGraph>> _actions;
        private readonly List<Action> _basicActions;

        #endregion Fields

        #region c'tor

        public Registry()
        {
            _actions = new List<Action<RegistrationGraph>>();
            _basicActions = new List<Action>();
        }

        #endregion c'tor

        public CreateRegistrationFamilyExpression<T> For<T>()
        {
            return new CreateRegistrationFamilyExpression<T>(this);
        }

        public OpenGenericFamilyExpression For(Type from)
        {
            return new OpenGenericFamilyExpression(from, this);
        }

        public void AddType(Type from, Type to, string name)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotEmpty(name, "name");

            _actions.Add(graph =>
                             {
                                 var family = graph.FindFamily(from);

                                 var registration = new TypeRegistration(from, to, name, new TransientLifetimeManager(),
                                                                         new InjectionMember[0]);

                                 family.AddRegistration(registration);
                             });
        }

        /// <summary>
        /// Imports the configuration from another registry into this registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void IncludeRegistry<T>() where T : Registry, new()
        {
            _actions.Add(g => new T().ConfigureRegistrationGraph(g));
        }

        /// <summary>
        /// Imports the configuration from another registry into this registry.
        /// </summary>
        /// <param name="registry"></param>
        public void IncludeRegistry(Registry registry)
        {
            Guard.AssertNotNull(registry, "registry");

            _actions.Add(registry.ConfigureRegistrationGraph);
        }

        /// <summary>
        /// Designates a policy for scanning assemblies to auto
        /// register types
        /// </summary>
        /// <returns></returns>
        public void Scan(Action<IAssemblyScanner> action)
        {
            var scanner = new AssemblyScanner();

            action(scanner);

            _actions.Add(graph => graph.AddScanner(scanner));
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

        internal void AddExpression(Action<RegistrationGraph> alteration)
        {
            Guard.AssertNotNull(alteration, "alteration");

            _actions.Add(alteration);
        }

        internal void ConfigureRegistrationGraph(RegistrationGraph graph)
        {
            Guard.AssertNotNull(graph, "graph");

            if (graph.Registries.Contains(this)) return;

            //graph.Log.StartSource("Registry:  " + GetType().AssemblyQualifiedName);

            _basicActions.ForEach(action => action());
            _actions.ForEach(action => action(graph));

            graph.Registries.Add(this);
        }

        public RegistrationGraph Build()
        {
            var graph = new RegistrationGraph();
            ConfigureRegistrationGraph(graph);

            return graph;
        }
    }
}
