namespace TecX.Unity.Proxies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public class NullObjectBuilder : ProxyBuilder
    {
        public NullObjectBuilder(Type contract, ModuleBuilder moduleBuilder)
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

            FieldInfo stringEmpty = typeof(string).GetField("Empty", BindingFlags.Static | BindingFlags.Public);

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

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i].IsOut)
                    {
                        Type parameterType = parameters[i].ParameterType.GetElementType();

                        if (parameterType == typeof(string))
                        {
                            il.Emit(OpCodes.Ldarg, i + 1);
                            il.Emit(OpCodes.Ldsfld, stringEmpty);
                            il.Emit(OpCodes.Stind_Ref);
                        }
                        else if (parameterType.IsGenericType)
                        {
                            Type openGeneric = parameterType.GetGenericTypeDefinition();

                            if (openGeneric == typeof(IList<>) ||
                                openGeneric == typeof(List<>) ||
                                openGeneric == typeof(ICollection<>))
                            {
                                ConstructorInfo ctor = typeof(List<>).MakeGenericType(parameterType.GetGenericArguments()).GetConstructor(Type.EmptyTypes);

                                il.Emit(OpCodes.Ldarg, i + 1);
                                il.Emit(OpCodes.Newobj, ctor);
                                il.Emit(OpCodes.Stind_Ref);
                            }

                            if (openGeneric == typeof(IEnumerable<>))
                            {
                                WriteArrayOutParam(parameterType, i, il);
                            }
                        }
                        else if (parameterType.IsArray)
                        {
                            WriteArrayOutParam(parameterType, i, il);
                        }
                    }
                }

                if (method.ReturnType == typeof(void))
                {
                    il.Emit(OpCodes.Ret);
                }
                else if (method.ReturnType.IsArray)
                {
                    WriteArrayReturnValue(il, method);
                }
                else if (method.ReturnType.IsGenericType)
                {
                    Type openGeneric = method.ReturnType.GetGenericTypeDefinition();

                    if (openGeneric == typeof(ICollection<>) ||
                        openGeneric == typeof(IList<>) ||
                        openGeneric == typeof(List<>))
                    {
                        WriteListReturnValue(il, method);
                    }

                    if (openGeneric == typeof(IEnumerable<>))
                    {
                        WriteArrayReturnValue(il, method);
                    }
                }
                else if (method.ReturnType == typeof(string))
                {
                    LocalBuilder lb = il.DeclareLocal(method.ReturnType);

                    var ret = il.DefineLabel();

                    il.Emit(OpCodes.Nop);
                    il.Emit(OpCodes.Ldsfld, stringEmpty);
                    il.Emit(OpCodes.Stloc_0);
                    il.Emit(OpCodes.Br_S, ret);
                    il.MarkLabel(ret);
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ret);
                }
                else
                {
                    LocalBuilder lb = il.DeclareLocal(method.ReturnType);

                    var ret = il.DefineLabel();

                    il.Emit(OpCodes.Ldloca_S, lb);
                    il.Emit(OpCodes.Initobj, method.ReturnType);
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Stloc_0);
                    il.Emit(OpCodes.Br_S, ret);
                    il.MarkLabel(ret);
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ret);
                }
            }
        }

        private static void WriteArrayOutParam(Type parameterType, int paramIndex, ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg, paramIndex + 1);

            Type arrayElementType = parameterType.GetElementType();
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Newarr, arrayElementType);
            il.Emit(OpCodes.Stind_Ref);
        }

        private static void WriteListReturnValue(ILGenerator il, MethodInfo method)
        {
            LocalBuilder lb = il.DeclareLocal(method.ReturnType);
            var ret = il.DefineLabel();

            ConstructorInfo ctor =
                typeof(List<>).MakeGenericType(method.ReturnType.GetGenericArguments()).GetConstructor(Type.EmptyTypes);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br_S, ret);
            il.MarkLabel(ret);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
        }

        private static void WriteArrayReturnValue(ILGenerator il, MethodInfo method)
        {
            LocalBuilder lb = il.DeclareLocal(method.ReturnType);

            var ret = il.DefineLabel();

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Newarr, typeof(string));
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br, ret);
            il.MarkLabel(ret);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
        }

        private TypeBuilder CreateTypeBuilder()
        {
            string name = "NullObjectImplementationOf_" + this.Contract.Name;

            TypeBuilder typeBuilder = this.ModuleBuilder.DefineType(
                name, Constants.TypeAttr, typeof(object), new[] { this.Contract });

            return typeBuilder;
        }
    }
}
