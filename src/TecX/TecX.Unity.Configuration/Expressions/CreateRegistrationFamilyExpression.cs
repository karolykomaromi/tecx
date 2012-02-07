namespace TecX.Unity.Configuration.Expressions
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class CreateRegistrationFamilyExpression : IExtensibleConfiguration
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
                    RegistrationFamily family = config.FindFamily(this.From);

                    this.alterations.ForEach(action => action(family));
                });
        }

        public Type From
        {
            get
            {
                return this.@from;
            }
        }

        public TypeRegistrationExpression Use<TTo>()
        {
            return Use(typeof(TTo));
        }

        public TypeRegistrationExpression Use(Type to)
        {
            Guard.AssertNotNull(to, "to");

            var expression = new TypeRegistrationExpression(this.From, to);

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

            var expression = new InstanceRegistrationExpression(this.From, instance);

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

            var expression = new NamedTypeRegistrationExpression(this.From, to);

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

            var expression = new NamedInstanceRegistrationExpression(this.From, instance);

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

        /// <summary>
        /// For configurationExtensions only!
        /// </summary>
        /// <param name="action"></param>
        void IExtensibleConfiguration.AddAlternation(Action<RegistrationFamily> action)
        {
            Guard.AssertNotNull(action, "action");

            this.alterations.Add(action);
        }
    }
}
