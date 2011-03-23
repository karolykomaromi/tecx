using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class CreateRegistrationFamilyExpression
    {
        private readonly Type _from;
        private readonly List<Action<RegistrationFamily>> _alterations;
        private readonly List<Action<RegistrationGraph>> _children;

        public CreateRegistrationFamilyExpression(Type from, Registry registry)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(registry, "registry");

            _from = from;

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
            return Use(typeof(TTo));
        }

        public TypeRegistrationExpression Use(Type to)
        {
            Guard.AssertNotNull(to, "to"); 
            
            var expression = new TypeRegistrationExpression(_from, to);

            _alterations.Add(family =>
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public InstanceRegistrationExpression Use(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new InstanceRegistrationExpression(_from, instance);

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
            return Add(typeof(TTo));
        }

        public NamedTypeRegistrationExpression Add(Type to)
        {
            Guard.AssertNotNull(to, "to"); 
            
            var expression = new NamedTypeRegistrationExpression(_from, to);

            _alterations.Add(family =>
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public NamedInstanceRegistrationExpression Add(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new NamedInstanceRegistrationExpression(_from, instance);

            _alterations.Add(family =>
            {
                var expr = expression;

                var registration = expr.Compile();

                family.AddRegistration(registration);
            });

            return expression;
        }

        public CreateRegistrationFamilyExpression LifetimeIs(Func<LifetimeManager> lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            _alterations.Add(family => family.LifetimeIs(lifetime));

            return this;
        }
    }
}
