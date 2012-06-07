namespace TecX.Unity.Configuration.Builders
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class RegistrationFamilyBuilder
    {
        private readonly Type @from;
        private readonly List<Action<RegistrationFamily>> alterations;

        public RegistrationFamilyBuilder(Type from, ConfigurationBuilder builder)
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

        public TypeRegistrationBuilder Use<TTo>()
        {
            return this.Use(typeof(TTo));
        }

        public TypeRegistrationBuilder Use(Type to)
        {
            Guard.AssertNotNull(to, "to");

            var expression = new TypeRegistrationBuilder(this.From, to);

            this.alterations.Add(family =>
            {
                var expr = expression;

                family.AddRegistration(expr);
            });

            return expression;
        }

        public InstanceRegistrationBuilder Use(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new InstanceRegistrationBuilder(this.From, instance);

            this.alterations.Add(family =>
            {
                var expr = expression;

                family.AddRegistration(expr);
            });

            return expression;
        }

        public TypeRegistrationBuilder Add<TTo>()
        {
            return this.Add(typeof(TTo));
        }

        public TypeRegistrationBuilder Add(Type to)
        {
            Guard.AssertNotNull(to, "to");

            var expression = new TypeRegistrationBuilder(this.From, to).Named(Guid.NewGuid().ToString());

            this.alterations.Add(family =>
            {
                var expr = expression;

                family.AddRegistration(expr);
            });

            return expression;
        }

        public InstanceRegistrationBuilder Add(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var expression = new InstanceRegistrationBuilder(this.From, instance).Named(Guid.NewGuid().ToString());

            this.alterations.Add(family =>
            {
                var expr = expression;

                family.AddRegistration(expr);
            });

            return expression;
        }

        public RegistrationFamilyBuilder LifetimeIs(Func<LifetimeManager> lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            this.alterations.Add(family => family.LifetimeIs(lifetime));

            return this;
        }

        /// <summary>
        /// For configurationExtensions only!
        /// </summary>
        /// <param name="action"></param>
        internal void AddAlternation(Action<RegistrationFamily> action)
        {
            Guard.AssertNotNull(action, "action");

            this.alterations.Add(action);
        }
    }
}
