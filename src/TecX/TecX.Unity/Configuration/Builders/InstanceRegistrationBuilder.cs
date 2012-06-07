namespace TecX.Unity.Configuration.Builders
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class InstanceRegistrationBuilder : RegistrationBuilder<InstanceRegistrationBuilder>
    {
        private readonly object instance; 

        public InstanceRegistrationBuilder(Type from, object instance)
            : base(from)
        {
            Guard.AssertNotNull(instance, "instance");

            this.instance = instance;

            this.LifetimeIs(new ContainerControlledLifetimeManager());
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