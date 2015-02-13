namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Reflection.Emit;

    public abstract class ProxyBuilder
    {
        private readonly ModuleBuilder moduleBuilder;

        private readonly Type contract;

        protected ProxyBuilder(ModuleBuilder moduleBuilder, Type contract)
        {
            System.Diagnostics.Contracts.Contract.Requires(moduleBuilder != null);
            System.Diagnostics.Contracts.Contract.Requires(contract != null);
            System.Diagnostics.Contracts.Contract.Requires(contract.IsInterface);

            this.moduleBuilder = moduleBuilder;
            this.contract = contract;
        }

        protected ModuleBuilder ModuleBuilder
        {
            get { return this.moduleBuilder; }
        }

        protected Type Contract
        {
            get { return this.contract; }
        }

        public abstract Type Build();

        protected abstract TypeBuilder CreateTypeBuilder();
    }
}