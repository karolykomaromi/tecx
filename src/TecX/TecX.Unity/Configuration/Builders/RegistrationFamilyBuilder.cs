namespace TecX.Unity.Configuration.Builders
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Factories;

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

        public TypeRegistrationBuilder AsFactory()
        {
            TypeRegistrationBuilder builder = this.Use(this.From);

            if (this.From.IsInterface)
            {
                builder.Enrich(x => x.Add(new TypedFactory()));
            }
            else if (typeof(Delegate).IsAssignableFrom(this.From))
            {
                builder.Enrich(x => x.Add(new DelegateFactory()));
            }
            else
            {
                string msg = string.Format("Type '{0}' is neither an interface nor a delegate.", this.From.FullName);

                throw new InvalidOperationException(msg);
            }

            return builder;
        }

        public TypeRegistrationBuilder AsFactory(ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(selector, "selector");

            if (!this.From.IsInterface)
            {
                throw new InvalidOperationException("From must be an interface.");
            }

            TypeRegistrationBuilder builder = this.Use(this.from);

            builder.Enrich(x => x.Add(new TypedFactory(selector)));

            return builder;
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
