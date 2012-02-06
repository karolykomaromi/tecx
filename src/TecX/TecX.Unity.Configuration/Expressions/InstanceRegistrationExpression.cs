namespace TecX.Unity.Configuration.Expressions
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.ContextualBinding;

    public class InstanceRegistrationExpression : RegistrationExpression<InstanceRegistrationExpression>
    {
        private readonly Type @from;

        private readonly object instance;

        private Func<InstanceRegistration> compile;

        public InstanceRegistrationExpression(Type from, object instance)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(instance, "instance");

            this.@from = from;
            this.instance = instance;

            LifetimeIs(new ContainerControlledLifetimeManager());

            this.compile = () => new InstanceRegistration(this.From, null, this.Instance, this.Lifetime);
        }

        public Type From
        {
            get { return this.@from; }
        }

        public object Instance
        {
            get { return this.instance; }
        }

        public override Registration Compile()
        {
            return this.compile();
        }

        public InstanceRegistrationExpression If(Predicate<IBindingContext, IBuilderContext> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            this.compile = () =>
                {
                    var p = predicate;
                    return new ContextualInstanceRegistration(this.From, null, this.Instance, this.Lifetime, p);
                };

            return this;
        }

        private class ContextualInstanceRegistration : InstanceRegistration
        {
            private readonly Predicate<IBindingContext, IBuilderContext> predicate;

            public ContextualInstanceRegistration(
                Type @from, 
                string name, 
                object instance, 
                LifetimeManager lifetime, 
                Predicate<IBindingContext, IBuilderContext> predicate)
                : base(@from, name, instance, lifetime)
            {
                Guard.AssertNotNull(predicate, "predicate");

                this.predicate = predicate;
            }

            public override void Configure(IUnityContainer container)
            {
                Guard.AssertNotNull(container, "container");

                container.RegisterInstance(this.From, this.Instance, this.predicate, this.Lifetime);
            }
        }
    }
}