namespace TecX.Unity.Configuration.Expressions
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class CreateRegistrationFamilyExpression
    {
        private readonly Type @from;
        private readonly List<Action<RegistrationFamily>> alterations;

        public CreateRegistrationFamilyExpression(Type from, ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(builder, "ConfigurationBuilder");

            this.@from = from;

            this.alterations = new List<Action<RegistrationFamily>>();

            builder.AddExpression(config =>
                {
                    RegistrationFamily family = config.FindFamily(this.@from);

                    this.alterations.ForEach(action => action(family));
                });
        }

        public TypeRegistrationExpression Use<TTo>()
        {
            return Use(typeof(TTo));
        }

        public TypeRegistrationExpression Use(Type to)
        {
            Guard.AssertNotNull(to, "to");

            var expression = new TypeRegistrationExpression(this.@from, to);

            this.alterations.Add(family =>
            {
                var expr = expression;

                family.AddRegistration(expr);
            });

            return expression;
        }

        public InstanceRegistrationExpression Use(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new InstanceRegistrationExpression(this.@from, instance);

            this.alterations.Add(family =>
            {
                var expr = expression;

                family.AddRegistration(expr);
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

            var expression = new NamedTypeRegistrationExpression(this.@from, to);

            this.alterations.Add(family =>
            {
                var expr = expression;

                family.AddRegistration(expr);
            });

            return expression;
        }

        public NamedInstanceRegistrationExpression Add(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new NamedInstanceRegistrationExpression(this.@from, instance);

            this.alterations.Add(family =>
            {
                var expr = expression;

                family.AddRegistration(expr);
            });

            return expression;
        }

        public CreateRegistrationFamilyExpression LifetimeIs(Func<LifetimeManager> lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            this.alterations.Add(family => family.LifetimeIs(lifetime));

            return this;
        }
    }
}
