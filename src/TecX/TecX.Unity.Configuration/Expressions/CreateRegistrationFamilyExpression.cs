using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class CreateRegistrationFamilyExpression<TFrom>
    {
        private readonly Type _from;
        private readonly List<Action<RegistrationFamily>> _alterations;
        private readonly List<Action<RegistrationGraph>> _children;

        public CreateRegistrationFamilyExpression(Registry registry)
        {
            Guard.AssertNotNull(registry, "registry");

            _from = typeof(TFrom);

            _alterations = new List<Action<RegistrationFamily>>();
            _children = new List<Action<RegistrationGraph>>();

            registry.AddExpression(graph =>
            {
                RegistrationFamily family = graph.FindFamily(_from);

                _children.ForEach(action => action(graph));
                _alterations.ForEach(action => action(family));
            });
        }

        public TypeRegistrationExpression Use<TTo>()
        {
            var expression = new TypeRegistrationExpression(_from, typeof(TTo));

            _alterations.Add(family => 
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public InstanceRegistrationExpression Use(TFrom instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new InstanceRegistrationExpression(typeof(TFrom), instance);

            _alterations.Add(family =>
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public NamedTypeRegistrationExpression Add<TTo>()
        {
            var expression = new NamedTypeRegistrationExpression(_from, typeof(TTo));

            _alterations.Add(family =>
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public NamedInstanceRegistrationExpression Add(TFrom instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new NamedInstanceRegistrationExpression(typeof(TFrom), instance);

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

            _alterations.Add(family => family.LifetimeIs(lifetime));

            return this;
        }
    }
}
