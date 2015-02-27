namespace Hydra.Unity.Validation
{
    using System;
    using System.Diagnostics.Contracts;
    using FluentValidation;
    using Microsoft.Practices.Unity;

    public class UnityValidatorFactory : ValidatorFactoryBase
    {
        private readonly IUnityContainer container;

        public UnityValidatorFactory(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return (IValidator)this.container.Resolve(validatorType);
        }
    }
}
