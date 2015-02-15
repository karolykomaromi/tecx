namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public abstract class ProxyBuilder
    {
        private readonly ModuleBuilder moduleBuilder;

        private readonly Type contract;

        private readonly Type target;

        protected ProxyBuilder(ModuleBuilder moduleBuilder, Type contract, Type target)
        {
            System.Diagnostics.Contracts.Contract.Requires(moduleBuilder != null);
            System.Diagnostics.Contracts.Contract.Requires(contract != null);
            System.Diagnostics.Contracts.Contract.Requires(contract.IsInterface);
            System.Diagnostics.Contracts.Contract.Requires(target != null);

            this.moduleBuilder = moduleBuilder;
            this.contract = contract;
            this.target = target;
        }

        protected ModuleBuilder ModuleBuilder
        {
            get { return this.moduleBuilder; }
        }

        protected Type Contract
        {
            get { return this.contract; }
        }

        protected Type Target
        {
            get { return this.target; }
        }

        public abstract Type Build();

        protected virtual TypeBuilder CreateTypeBuilder()
        {
            string name = this.GetTypeName();

            TypeBuilder typeBuilder = this.ModuleBuilder.DefineType(
                name,
                Constants.Attributes.GeneratedType,
                typeof(object),
                new[] { this.Contract });

            return typeBuilder;
        }

        protected abstract string GetTypeName();

        protected virtual string GetGetMethodName(MethodInfo getterOnContract)
        {
            return this.Contract.FullName + "." + getterOnContract.Name;
            //return getterOnContract.Name;
        }

        protected virtual string GetSetMethodName(MethodInfo setterOnContract)
        {
            return this.Contract.FullName + "." + setterOnContract.Name;
            //return setterOnContract.Name;
        }

        protected virtual string GetMethodName(MethodInfo methodOnContract)
        {
            return this.Contract.FullName + "." + methodOnContract.Name;
        }
    }
}