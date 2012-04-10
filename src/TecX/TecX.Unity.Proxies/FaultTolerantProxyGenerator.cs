namespace TecX.Unity.Proxies
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.ServiceModel;

    using TecX.Common;

    public class FaultTolerantProxyGenerator
    {
        public static class Constants
        {
            /// <summary>
            /// The property "set" and property "get" methods require a special
            /// set of attributes.
            /// </summary>
            public const MethodAttributes GetSetAttributes = MethodAttributes.Public | 
                                                             MethodAttributes.SpecialName | 
                                                             MethodAttributes.HideBySig;

            public const string ProxyNamePostfix = "_FaultTolerantProxy";

            public const string FactoryFieldName = "factory";

            public const string ChannelFieldName = "channel";

            public const string ChannelPropertyName = "Channel";

            public const TypeAttributes TypeAttributes = TypeAttributes.Public |
                                                         TypeAttributes.Class |
                                                         TypeAttributes.AutoClass |
                                                         TypeAttributes.AnsiClass |
                                                         TypeAttributes.BeforeFieldInit |
                                                         TypeAttributes.AutoLayout;
        }

        private readonly ModuleBuilder moduleBuilder;

        private readonly Type contract;

        private readonly Type contractFactory;

        private FieldBuilder channelFieldBuilder;

        private FieldBuilder factoryFieldBuilder;

        private PropertyBuilder channelPropertyBuilder;

        private MethodBuilder channelGetterMethodBuilder;

        public FaultTolerantProxyGenerator(ModuleBuilder moduleBuilder, Type contract)
        {
            Guard.AssertNotNull(moduleBuilder, "moduleBuilder");
            Guard.AssertNotNull(contract, "contract");

            AssertIsInterface(contract);

            this.moduleBuilder = moduleBuilder;
            this.contract = contract;
            this.contractFactory = typeof(Func<>).MakeGenericType(contract);
        }

        public Type Generate()
        {
            var typeBuilder = this.CreateTypeBuilder();

            this.GenerateFields(typeBuilder);

            this.GenerateChannelProperty(typeBuilder);

            this.GenerateConstructor(typeBuilder);

            this.GenerateMethods(this.contract, typeBuilder);

            var proxyType = typeBuilder.CreateType();

            return proxyType;
        }

        private static void AssertIsInterface(Type type)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException(string.Format("Type {0} is not an interface", type.FullName));
            }
        }

        private void GenerateMethods(Type contract, TypeBuilder typeBuilder)
        {
            var methods = contract.GetMethods();

            var attributes = MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot |
                MethodAttributes.Virtual |
                MethodAttributes.Final;

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                var methodBuilder = typeBuilder.DefineMethod(
                     method.Name,
                     attributes,
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
                il.Emit(OpCodes.Call, this.channelGetterMethodBuilder);

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
            this.channelGetterMethodBuilder = typeBuilder.DefineMethod("get_Channel", Constants.GetSetAttributes, this.contract, Type.EmptyTypes);

            ILGenerator il = this.channelGetterMethodBuilder.GetILGenerator();

            var endif = il.DefineLabel();
            var ret = il.DefineLabel();

            il.DeclareLocal(this.contract);
            il.DeclareLocal(typeof(bool));

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, this.channelGetterMethodBuilder);
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
            MethodBuilder channelSetAccessor = typeBuilder.DefineMethod(
                "set_Channel", Constants.GetSetAttributes, null, new[] { this.contract });

            ILGenerator il = channelSetAccessor.GetILGenerator();

            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, this.channelFieldBuilder);
            il.Emit(OpCodes.Ret);
            this.channelPropertyBuilder.SetSetMethod(channelSetAccessor);
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
            MethodInfo meth = typeof(Guard).GetMethod("AssertNotNull", BindingFlags.Static | BindingFlags.Public);
            il.Emit(OpCodes.Call, meth);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, this.factoryFieldBuilder);
        }

        private TypeBuilder CreateTypeBuilder()
        {
            string name = this.contract.Name.TrimStart(new[] { 'I' }) + Constants.ProxyNamePostfix;

            TypeBuilder typeBuilder = this.moduleBuilder.DefineType(
                name,
                Constants.TypeAttributes,
                typeof(object),
                new[] { this.contract });

            return typeBuilder;
        }
    }
}
