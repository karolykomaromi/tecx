namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    public class ProxyGenerator
    {
        private readonly AssemblyBuilder assemblyBuilder;

        private readonly ModuleBuilder moduleBuilder;

        private readonly ConcurrentDictionary<Tuple<Type, Type>, Type> ducks;

        private readonly ConcurrentDictionary<Type, Type> lazies;

        private readonly ConcurrentDictionary<Type, Type> decoraptors;

        private readonly ConcurrentDictionary<Type, Type> decomissioningDecoraptors;

        public ProxyGenerator()
        {
            AssemblyName assemblyName = new AssemblyName { Name = Constants.Names.AssemblyName };

            AppDomain thisDomain = Thread.GetDomain();

            this.assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule(
                this.assemblyBuilder.GetName().Name, 
                Constants.Names.AssemblyFileName);

            this.ducks = new ConcurrentDictionary<Tuple<Type, Type>, Type>();

            this.lazies = new ConcurrentDictionary<Type, Type>();

            this.decoraptors = new ConcurrentDictionary<Type, Type>();

            this.decomissioningDecoraptors = new ConcurrentDictionary<Type, Type>();
        }

        public Type CreateDuckTypeProxy(Type contract, Type adaptee)
        {
            Contract.Requires(contract != null);
            Contract.Requires(adaptee != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            Type proxyType = this.ducks.GetOrAdd(
                new Tuple<Type, Type>(contract, adaptee),
                t =>
                {
                    var builder = new DuckTypeAdapterProxyBuilder(this.moduleBuilder, t.Item1, t.Item2);

                    Type pt = builder.Build();

                    return pt;
                });

            return proxyType;
        }

        public Type CreateLazyProxy(Type contract)
        {
            Contract.Requires(contract != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            Type proxyType = this.lazies.GetOrAdd(
                contract, 
                c =>
                {
                    var builder = new LazyProxyBuilder(this.moduleBuilder, c);

                    Type pt = builder.Build();

                    return pt;
                });

            return proxyType;
        }

        public Type CreateDecoraptorProxy(Type contract)
        {
            Contract.Requires(contract != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            Type proxyType = this.decoraptors.GetOrAdd(
                contract, 
                c =>
                {
                    var builder = new DecoraptorProxyBuilder(this.moduleBuilder, c);

                    Type pt = builder.Build();

                    return pt;
                });

            return proxyType;
        }

        public Type CreateDecomissioningDecoraptorProxy(Type contract)
        {
            Contract.Requires(contract != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            Type proxyType = this.decomissioningDecoraptors.GetOrAdd(
                contract, 
                c =>
                {
                    var builder = new DecomissioningDecoraptorProxyBuilder(this.moduleBuilder, c);

                    Type pt = builder.Build();

                    return pt;
                });

            return proxyType;
        }

        public void Save()
        {
            this.assemblyBuilder.Save(Constants.Names.AssemblyFileName);
        }
    }
}