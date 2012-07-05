namespace TecX.Unity.Proxies
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ProxyGeneratorExtension : UnityContainerExtension, IProxyGenerator
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
        private static class Constants
        {
            /// <summary>
            /// GeneratedProxies
            /// </summary>
            public const string AssemblyName = "GeneratedProxies";

            /// <summary>
            /// GeneratedProxies.dll
            /// </summary>
            public const string AssemblyFileName = AssemblyName + ".dll";
        }

        private readonly AssemblyBuilder assemblyBuilder;

        private readonly ModuleBuilder moduleBuilder;

        public ProxyGeneratorExtension()
        {
            AssemblyName assemblyName = new AssemblyName { Name = Constants.AssemblyName };

            AppDomain thisDomain = Thread.GetDomain();

            this.assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule(
                this.assemblyBuilder.GetName().Name, Constants.AssemblyFileName);
        }

        ////public Type CreateFaultTolerantProxy(Type contract)
        ////{
        ////    Guard.AssertNotNull(contract, "contract");

        ////    NamedTypeBuildKey key = new NamedTypeBuildKey(contract);

        ////    Type proxyType;
        ////    if (!this.faultTolerantProxies.TryGetValue(key, out proxyType))
        ////    {
        ////        var proxyGenerator = new FaultTolerantProxyGenerator(contract, this.moduleBuilder);

        ////        proxyType = proxyGenerator.Generate();

        ////        this.faultTolerantProxies[key] = proxyType;
        ////    }

        ////    this.assemblyBuilder.Save(Constants.AssemblyFileName);

        ////    return proxyType;
        ////}
        
        public Type CreateInterfaceProxyWithoutTargetDummy(Type contract)
        {
            Guard.AssertNotNull(contract, "contract");

            var builder = new InterfaceProxyWithoutTargetBuilder(contract, this.moduleBuilder);

            var proxyType = builder.Build();

            this.assemblyBuilder.Save(Constants.AssemblyFileName);

            return proxyType;
        }

        public Type CreateLazyInstantiationProxy(Type contract)
        {
            Guard.AssertNotNull(contract, "contract");

            var builder = new LazyProxyBuilder(contract, this.moduleBuilder);

            var proxyType = builder.Build();

            this.assemblyBuilder.Save(Constants.AssemblyFileName);

            return proxyType;
        }

        protected override void Initialize()
        {
        }
    }
}