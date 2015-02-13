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

        public Type CreateAdapterType(Type contract, Type adaptee)
        {
            Contract.Requires(contract != null);
            Contract.Requires(adaptee != null);

            var builder = new AdapterProxyBuilder(contract, adaptee, this.moduleBuilder);

            var proxyType = builder.Build();

            this.assemblyBuilder.Save(Constants.Names.AssemblyFileName);

            return proxyType;
        }
    }
}