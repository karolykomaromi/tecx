﻿namespace TecX.Unity.Proxies
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.ServiceModel;

    using TecX.Common;

    public class FaultTolerantProxyGenerator : ProxyGenerator
    {
        private static class Constants
        {
            public const string ProxyNamePostfix = "_FaultTolerantProxy";

            public const string FactoryFieldName = "factory";

            public const string ChannelFieldName = "channel";

            public const string ChannelPropertyName = "Channel";
        }

        private FieldBuilder channelFieldBuilder;

        private FieldBuilder factoryFieldBuilder;

        private PropertyBuilder channelPropertyBuilder;

        private MethodBuilder channelGetterMethodBuilder;

        private MethodBuilder channelSetterMethodBuilder;

        public FaultTolerantProxyGenerator(Type contract, ModuleBuilder moduleBuilder)
            : base(contract, moduleBuilder)
        {
        }

        public override Type Generate()
        {
            var typeBuilder = this.CreateTypeBuilder();

            this.GenerateFields(typeBuilder);

            this.GenerateChannelProperty(typeBuilder);

            this.GenerateConstructor(typeBuilder);

            this.GenerateMethods(this.contract, typeBuilder, this.channelGetterMethodBuilder);

            var proxyType = typeBuilder.CreateType();

            return proxyType;
        }

        private void GenerateFields(TypeBuilder typeBuilder)
        {
            this.channelFieldBuilder = typeBuilder.DefineField(Constants.ChannelFieldName, this.contract, FieldAttributes.Private);
            this.factoryFieldBuilder = typeBuilder.DefineField(Constants.FactoryFieldName, this.contractFactory, FieldAttributes.Private | FieldAttributes.InitOnly);
        }

        private void GenerateChannelProperty(TypeBuilder typeBuilder)
        {
            this.channelPropertyBuilder = typeBuilder.DefineProperty(
                Constants.ChannelPropertyName,
                PropertyAttributes.None,
                CallingConventions.Standard,
                this.contract,
                null);

            this.GenerateChannelGetter(typeBuilder);

            this.GenerateChannelSetter(typeBuilder);
        }

        private void GenerateChannelGetter(TypeBuilder typeBuilder)
        {
            this.channelGetterMethodBuilder = typeBuilder.DefineMethod("get_Channel", Proxies.Constants.GetSetAttributes, this.contract, Type.EmptyTypes);

            ILGenerator il = this.channelGetterMethodBuilder.GetILGenerator();

            var endif = il.DefineLabel();
            var ret = il.DefineLabel();

            il.DeclareLocal(this.contract);
            il.DeclareLocal(typeof(bool));

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.channelFieldBuilder);
            il.Emit(OpCodes.Castclass, typeof(ICommunicationObject));

            PropertyInfo state = typeof(ICommunicationObject).GetProperty("State");

            il.Emit(OpCodes.Callvirt, state.GetGetMethod());
            il.Emit(OpCodes.Ldc_I4_5);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc_1);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Brtrue_S, endif);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.factoryFieldBuilder);

            var invoke = this.contractFactory.GetMethod("Invoke");
            il.Emit(OpCodes.Callvirt, invoke);
            il.Emit(OpCodes.Stfld, this.channelFieldBuilder);
            il.Emit(OpCodes.Nop);

            il.MarkLabel(endif);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.channelFieldBuilder);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br_S, ret);
            il.MarkLabel(ret);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            this.channelPropertyBuilder.SetGetMethod(this.channelGetterMethodBuilder);
        }

        private void GenerateChannelSetter(TypeBuilder typeBuilder)
        {
            this.channelSetterMethodBuilder = typeBuilder.DefineMethod(
                "set_Channel", Proxies.Constants.GetSetAttributes, null, new[] { this.contract });

            ILGenerator il = this.channelSetterMethodBuilder.GetILGenerator();

            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, this.channelFieldBuilder);
            il.Emit(OpCodes.Ret);
            this.channelPropertyBuilder.SetSetMethod(this.channelSetterMethodBuilder);
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

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, this.factoryFieldBuilder);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            var invoke = this.contractFactory.GetMethod("Invoke");
            il.Emit(OpCodes.Callvirt, invoke);
            il.Emit(OpCodes.Stfld, this.channelFieldBuilder);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
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
