using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class InstanceRegistrationExpression<TFrom> : RegistrationExpression<InstanceRegistrationExpression<TFrom>>
    {
        private readonly object _instance;

        public InstanceRegistrationExpression(object instance)
        {
            Guard.AssertNotNull(instance, "instance");
            _instance = instance;

            _lifetimeFactory = () => new ContainerControlledLifetimeManager();
        }

        public override Registration Compile()
        {
            throw new NotImplementedException();
        }
    }
}