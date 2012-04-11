namespace TecX.Unity.Proxies
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using TecX.Common;

    public abstract class ProxyGenerator
    {
        protected ModuleBuilder moduleBuilder;

        protected Type contract;

        protected Type contractFactory;

        protected ProxyGenerator(Type contract, ModuleBuilder moduleBuilder)
        {
            Guard.AssertNotNull(contract, "contract");
            Guard.AssertNotNull(moduleBuilder, "moduleBuilder");

            AssertIsInterface(contract);

            this.contract = contract;
            this.moduleBuilder = moduleBuilder;
            this.contractFactory = typeof(Func<>).MakeGenericType(contract);
        }

        public abstract Type Generate();

        private static void AssertIsInterface(Type type)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException(string.Format("Type {0} is not an interface", type.FullName));
            }
        }

        protected void GenerateMethods(Type contract, TypeBuilder typeBuilder, MethodBuilder getter)
        {
            var methods = contract.GetMethods();

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                var methodBuilder = typeBuilder.DefineMethod(
                    method.Name,
                    Constants.MethodAttr,
                    CallingConventions.HasThis,
                    method.ReturnType,
                    parameters.Select(p => p.ParameterType).ToArray());

                for (int i = 0; i < parameters.Length; i++)
                {
                    methodBuilder.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Name);
                }

                var il = methodBuilder.GetILGenerator();
                if (method.ReturnType != typeof(void))
                {
                    il.DeclareLocal(method.ReturnType);
                }

                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, getter);

                for (int i = 1; i <= parameters.Length; i++)
                {
                    il.Emit(OpCodes.Ldarg, i);
                }

                il.Emit(OpCodes.Callvirt, method);

                if (method.ReturnType != typeof(void))
                {
                    il.Emit(OpCodes.Stloc_0);
                    var local = il.DefineLabel();
                    il.Emit(OpCodes.Br_S, local);
                    il.MarkLabel(local);
                    il.Emit(OpCodes.Ldloc_0);
                }

                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ret);
            }
        }
    }
}