namespace TecX.Unity.Proxies
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ProxyGeneratorExtension : UnityContainerExtension, IProxyGenerator
    {
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

            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule(this.assemblyBuilder.GetName().Name, Constants.AssemblyFileName);
        }

        protected override void Initialize()
        {
            
        }

        public Type CreateFaultTolerantProxy(Type contract)
        {
            Guard.AssertNotNull(contract, "contract");

            var proxyGenerator = new FaultTolerantProxyGenerator(this.moduleBuilder, contract);

            Type proxyType = proxyGenerator.Generate();

            this.assemblyBuilder.Save(Constants.AssemblyFileName);

            return proxyType;
        }
    }

    public interface IProxyGenerator : IUnityContainerExtensionConfigurator
    {
        Type CreateFaultTolerantProxy(Type contract);
    }
}
