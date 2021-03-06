﻿namespace Hydra.Composition
{
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Hydra.Filters;
    using Hydra.Unity.Validation;
    using Microsoft.Practices.Unity;

    public class FluentValidationConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Contract.Assume(GlobalFilters.Filters != null);

            this.Container.RegisterType<IValidatorFactory, UnityValidatorFactory>(new ContainerControlledLifetimeManager());

            this.Container.RegisterTypes(new ValidatorRegistrationConvention());

            GlobalFilters.Filters.Add(new ValidatorActionFilter());

            FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = this.Container.Resolve<IValidatorFactory>());
        }
    }
}