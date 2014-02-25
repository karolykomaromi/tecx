namespace Infrastructure.UnityExtensions.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Unity;

    public class CompositeConvention : IRegistrationConvention
    {
        private readonly HashSet<IRegistrationConvention> conventions;

        public CompositeConvention(params IRegistrationConvention[] conventions)
        {
            this.conventions = new HashSet<IRegistrationConvention>(conventions ?? new IRegistrationConvention[0]);
        }

        public void RegisterOnMatch(IUnityContainer container, Type type)
        {
            foreach (var convention in this.conventions)
            {
                convention.RegisterOnMatch(container, type);
            }
        }

        public void Add(IRegistrationConvention convention)
        {
            Contract.Requires(convention != null);

            this.conventions.Add(convention);
        }
    }
}