namespace TecX.Unity.Proxies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;


    public interface IProxyGeneratorPolicy : IBuilderPolicy
    {
        Type GenerateDummyImplementation(Type interfaceToProxy);
    }

    public class ProxyGeneratorPolicy : IProxyGeneratorPolicy
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

        private readonly IDictionary<Type, Type> dummies;

        private readonly AssemblyBuilder assemblyBuilder;

        private readonly ModuleBuilder moduleBuilder;

        public ProxyGeneratorPolicy()
        {
            AssemblyName assemblyName = new AssemblyName { Name = Constants.AssemblyName };

            AppDomain thisDomain = Thread.GetDomain();

            this.assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule(
                this.assemblyBuilder.GetName().Name, Constants.AssemblyFileName);

            this.dummies = new Dictionary<Type, Type>();
        }

        public Type GenerateDummyImplementation(Type interfaceToProxy)
        {
            Guard.AssertNotNull(interfaceToProxy, "interfaceToProxy");
            Guard.AssertIsInterface(interfaceToProxy, "interfaceToProxy");

            Type dummy;
            if (!this.dummies.TryGetValue(interfaceToProxy, out dummy))
            {
                var builder = new InterfaceProxyWithoutTargetBuilder(interfaceToProxy, this.moduleBuilder);

                dummy = builder.Build();

                this.dummies.Add(interfaceToProxy, dummy);
            }
#if DEBUG
            this.assemblyBuilder.Save(Constants.AssemblyFileName);
#endif
            return dummy;
        }
    }
}
