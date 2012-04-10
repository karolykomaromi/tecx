namespace TecX.Unity.Proxies
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    using Microsoft.Practices.Unity;

    public class ProxyGeneratorExtension : UnityContainerExtension
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
    }
}
