using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;
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
    }
}
