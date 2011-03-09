using System;
using System.Collections.Generic;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class CreateRegistrationFamilyExpression<TFrom>
    {
        private readonly Registry _registry;
        private readonly Type _from;
        private readonly List<Action<RegistrationFamily>> _alterations;
        private readonly List<Action<RegistrationGraph>> _children;

        public CreateRegistrationFamilyExpression(Registry registry)
        {
            Guard.AssertNotNull(registry, "registry");

            _registry = registry;
            _from = typeof(TFrom);

            _alterations = new List<Action<RegistrationFamily>>();
            _children = new List<Action<RegistrationGraph>>();

            _registry.AddExpression(graph =>
            {
                RegistrationFamily family = graph.FindFamily(_from);

                _children.ForEach(action => action(graph));
                _alterations.ForEach(action => action(family));
            });
        }

        public TypeRegistrationExpression<TFrom, TTo> Use<TTo>()
        {
            var expression = new TypeRegistrationExpression<TFrom, TTo>();

            _alterations.Add(family => 
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public InstanceRegistrationExpression<TFrom> Use(TFrom instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new InstanceRegistrationExpression<TFrom>(instance);

            _alterations.Add(family =>
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public NamedTypeRegistrationExpression<TFrom, TTo> Add<TTo>()
        {
            var expression = new NamedTypeRegistrationExpression<TFrom, TTo>();

            _alterations.Add(family =>
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public NamedInstanceRegistrationExpression<TFrom> Add(TFrom instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new NamedInstanceRegistrationExpression<TFrom>(instance);

            _alterations.Add(family =>
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public CreateRegistrationFamilyExpression<TFrom> LifetimeIs(Func<LifetimeManager> lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            //_children.Add(graph =>
            //{
            //    RegistrationFamily family = graph.FindFamily(_from);

            //    var lt = lifetime;

            //    family.Registrations.ForEach(registration => registration.Lifetime = lt());
            //});

            _alterations.Add(family =>
                                 {
                                     var lt = lifetime;

                                     family.Registrations.ForEach(registration => registration.Lifetime = lt());
                                 });

            return this;
        }
    }
}
