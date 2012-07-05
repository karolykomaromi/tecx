namespace TecX.Unity.Proxies
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public class InterfaceProxyWithoutTargetBuilder : ProxyBuilder
    {
        public InterfaceProxyWithoutTargetBuilder(Type contract, ModuleBuilder moduleBuilder)
            : base(contract, moduleBuilder)
        {
        }

        public override Type Build()
        {
            var typeBuilder = this.CreateTypeBuilder();

            this.GenerateConstructor(typeBuilder);

            this.GenerateMethods(this.Contract, typeBuilder);

            var proxyType = typeBuilder.CreateType();

            return proxyType;
        }

        private void GenerateConstructor(TypeBuilder typeBuilder)
        {
            ConstructorBuilder constructor =
                typeBuilder.DefineConstructor(
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                    CallingConventions.Standard,
                    Type.EmptyTypes);

            // Define the reflection ConstructorInfor for System.Object
            ConstructorInfo ci = typeof(object).GetConstructor(new Type[0]);

            // call constructor of base object
            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, ci);
            il.Emit(OpCodes.Ret);
        }

        private void GenerateMethods(Type contract, TypeBuilder typeBuilder)
        {
            var methods = contract.GetMethods();

            ConstructorInfo notImplementedExCtor = typeof(NotImplementedException).GetConstructor(Type.EmptyTypes);

            foreach (MethodInfo method in methods)
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

                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Newobj, notImplementedExCtor);
                il.Emit(OpCodes.Throw);
            }
        }

        private TypeBuilder CreateTypeBuilder()
        {
            string name = "DummyImplementationOf_" + this.Contract.Name;

            TypeBuilder typeBuilder = this.ModuleBuilder.DefineType(
                name, Proxies.Constants.TypeAttr, typeof(object), new[] { this.Contract });

            return typeBuilder;
        }
    }
}
