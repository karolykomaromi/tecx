namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    public class ProxyGenerator
    {
        private readonly AssemblyBuilder assemblyBuilder;

        private readonly ModuleBuilder moduleBuilder;

        public ProxyGenerator()
        {
            AssemblyName assemblyName = new AssemblyName { Name = Constants.Names.AssemblyName };

            AppDomain thisDomain = Thread.GetDomain();

            this.assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule(
                this.assemblyBuilder.GetName().Name, Constants.Names.AssemblyFileName);
        }

        public Type CreateDuckTypeProxy(Type contract, Type adaptee)
        {
            Contract.Requires(contract != null);
            Contract.Requires(adaptee != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            ProxyBuilder builder = new DuckTypeAdapterProxyBuilder(this.moduleBuilder, contract, adaptee);

            Type proxyType = builder.Build();

            this.assemblyBuilder.Save(Constants.Names.AssemblyFileName);

            return proxyType;
        }

        public Type CreateLazyProxy(Type contract)
        {
            Contract.Requires(contract != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            ProxyBuilder builder = new LazyProxyBuilder(this.moduleBuilder, contract);

            Type proxyType = builder.Build();

            this.assemblyBuilder.Save(Constants.Names.AssemblyFileName);

            return proxyType;
        }

        public Type CreateDecoraptorProxy(Type contract)
        {
            Contract.Requires(contract != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            ProxyBuilder builder = new DecoraptorProxyBuilder(this.moduleBuilder, contract);

            Type proxyType = builder.Build();

            this.assemblyBuilder.Save(Constants.Names.AssemblyFileName);

            return proxyType;
        }
    }
}