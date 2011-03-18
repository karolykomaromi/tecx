using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class InstanceRegistrationExpression : RegistrationExpression<InstanceRegistrationExpression>
    {
        private readonly Type _from;
        private readonly object _instance;

        public Type From
        {
            get { return _from; }
        }

        public object Instance
        {
            get { return _instance; }
        }

        public InstanceRegistrationExpression(Type from, object instance)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(instance, "instance");

            _from = from;
            _instance = instance;

            LifetimeIs(() => new ContainerControlledLifetimeManager());
        }

        public override Registration Compile()
        {
            return new InstanceRegistration(From, null, _instance, Lifetime);
        }
    }
}