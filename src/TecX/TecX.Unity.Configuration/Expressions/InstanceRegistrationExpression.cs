namespace TecX.Unity.Configuration.Expressions
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class InstanceRegistrationExpression : RegistrationExpression<InstanceRegistrationExpression>
    {
        private readonly Type @from;

        private readonly object instance;

        public InstanceRegistrationExpression(Type from, object instance)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(instance, "instance");

            this.@from = from;
            this.instance = instance;

            LifetimeIs(new ContainerControlledLifetimeManager());
        }

        public Type From
        {
            get { return this.@from; }
        }

        public object Instance
        {
            get { return this.instance; }
        }

        protected override Registration DefaultCompilationStrategy()
        {
            return new InstanceRegistration(this.From, null, this.Instance, this.Lifetime);
        }
    }
}