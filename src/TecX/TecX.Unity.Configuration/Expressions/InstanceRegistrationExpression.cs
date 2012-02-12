namespace TecX.Unity.Configuration.Expressions
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class InstanceRegistrationExpression : RegistrationExpression<InstanceRegistrationExpression>
    {
        private readonly object instance; 

        public InstanceRegistrationExpression(Type from, object instance)
            : base(from)
        {
            Guard.AssertNotNull(instance, "instance");

            this.instance = instance;

            LifetimeIs(new ContainerControlledLifetimeManager());
        }

        public object Instance
        {
            get { return this.instance; }
        }

        protected override Registration DefaultCompilationStrategy()
        {
            return new InstanceRegistration(this.From, this.Name, this.Instance, this.Lifetime);
        }
    }
}