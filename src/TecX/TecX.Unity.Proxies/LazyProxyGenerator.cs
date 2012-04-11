﻿namespace TecX.Unity.Proxies
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using TecX.Common;

    public class LazyProxyGenerator : ProxyGenerator
    {
        private readonly Type lazyInstanceType;

        private FieldBuilder instanceFieldBuilder;

        private PropertyBuilder instancePropertyBuilder;

        private MethodBuilder instanceGetterMethodBuilder;

        private static class Constants
        {
            public const string ProxyNamePostfix = "_LazyInstantiationProxy";

            public const string InstanceFieldName = "instance";

            public const string InstancePropertyName = "Instance";

            public const string FactoryFieldName = "factory";
        }

        public LazyProxyGenerator(Type contract, ModuleBuilder moduleBuilder)
            : base(contract, moduleBuilder)
        {
            this.lazyInstanceType = typeof(Lazy<>).MakeGenericType(contract);
        }

        public override Type Generate()
        {
            var typeBuilder = this.CreateTypeBuilder();

            this.GenerateFields(typeBuilder);

            this.GenerateConstructor(typeBuilder);

            this.GenerateInstanceProperty(typeBuilder);

            this.GenerateMethods(this.contract, typeBuilder, this.instanceGetterMethodBuilder);

            var proxyType = typeBuilder.CreateType();

            return proxyType;
        }

        private void GenerateInstanceProperty(TypeBuilder typeBuilder)
        {
            this.instancePropertyBuilder = typeBuilder.DefineProperty(
                Constants.InstancePropertyName,
                PropertyAttributes.None,
                CallingConventions.Standard,
                this.contract,
                null);

            this.GenerateInstanceGetter(typeBuilder);
        }

        private void GenerateInstanceGetter(TypeBuilder typeBuilder)
        {
            this.instanceGetterMethodBuilder = typeBuilder.DefineMethod("get_Instance", Proxies.Constants.GetSetAttributes, this.contract, Type.EmptyTypes);

            ILGenerator il = this.instanceGetterMethodBuilder.GetILGenerator();
            var ret = il.DefineLabel();

            il.DeclareLocal(this.contract);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.instanceFieldBuilder);

            var getter = this.lazyInstanceType.GetProperty("Value").GetGetMethod();

            il.Emit(OpCodes.Callvirt, getter);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br_S, ret);
            il.MarkLabel(ret);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            this.instancePropertyBuilder.SetGetMethod(this.instanceGetterMethodBuilder);
        }

        private void GenerateFields(TypeBuilder typeBuilder)
        {
            this.instanceFieldBuilder = typeBuilder.DefineField(Constants.InstanceFieldName, this.lazyInstanceType, FieldAttributes.Private);
        }

        private void GenerateConstructor(TypeBuilder typeBuilder)
        {
            ConstructorBuilder constructor =
                typeBuilder.DefineConstructor(
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                    CallingConventions.Standard,
                    new[] { this.contractFactory });

            constructor.DefineParameter(1, ParameterAttributes.None, Constants.FactoryFieldName);

            // Define the reflection ConstructorInfor for System.Object
            ConstructorInfo ci = typeof(object).GetConstructor(new Type[0]);

            // call constructor of base object
            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, ci);

            // guard clause for factory parameter
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, Constants.FactoryFieldName);

            MethodInfo meth = typeof(Guard).GetMethod(
                "AssertNotNull",
                BindingFlags.Static | BindingFlags.Public,
                (Binder)null,
                new[] { typeof(object), typeof(string) },
                (ParameterModifier[])null);

            il.Emit(OpCodes.Call, meth);

            ConstructorInfo funcCtor =
                this.lazyInstanceType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public,
                    (Binder)null,
                    new[] { this.contractFactory },
                    (ParameterModifier[])null);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Newobj, funcCtor);
            il.Emit(OpCodes.Stfld, this.instanceFieldBuilder);
        }

        private TypeBuilder CreateTypeBuilder()
        {
            string name = this.contract.Name.TrimStart(new[] { 'I' }) + Constants.ProxyNamePostfix;

            TypeBuilder typeBuilder = this.moduleBuilder.DefineType(
                name,
                Proxies.Constants.TypeAttr,
                typeof(object),
                new[] { this.contract });

            return typeBuilder;
        }
    }
}
