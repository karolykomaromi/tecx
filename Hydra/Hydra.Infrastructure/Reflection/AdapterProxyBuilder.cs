namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public class AdapterProxyBuilder
    {
        private readonly ModuleBuilder moduleBuilder;
        private readonly Type contract;
        private readonly Type adaptee;

        public AdapterProxyBuilder(Type contract, Type adaptee, ModuleBuilder moduleBuilder)
        {
            System.Diagnostics.Contracts.Contract.Requires(contract != null);
            System.Diagnostics.Contracts.Contract.Requires(adaptee != null);
            System.Diagnostics.Contracts.Contract.Requires(moduleBuilder != null);

            AssertIsInterface(contract);

            this.contract = contract;
            this.adaptee = adaptee;
            this.moduleBuilder = moduleBuilder;
        }

        protected ModuleBuilder ModuleBuilder
        {
            get { return this.moduleBuilder; }
        }

        protected Type Contract
        {
            get { return this.contract; }
        }

        protected Type Adaptee
        {
            get { return this.adaptee; }
        }

        public Type Build()
        {
            TypeBuilder typeBuilder = this.CreateTypeBuilder();

            FieldBuilder adapteeField = this.GenerateAdapteeField(typeBuilder);

            this.GenerateConstructor(typeBuilder, adapteeField);

            this.GenerateDelegatingProperties(typeBuilder, adapteeField);

            this.GenerateDelegatingMethods(typeBuilder, adapteeField);

            Type adapterType = typeBuilder.CreateType();

            return adapterType;
        }

        private static void AssertIsInterface(Type type)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException(string.Format("Type {0} is not an interface", type.FullName));
            }
        }

        private TypeBuilder CreateTypeBuilder()
        {
            string name = this.Adaptee.Name + "To" + this.Contract.Name + "Adapter";

            TypeBuilder typeBuilder = this.ModuleBuilder.DefineType(
                name,
                Constants.Attributes.GeneratedType,
                typeof(object),
                new[] { this.Contract });

            return typeBuilder;
        }

        private FieldBuilder GenerateAdapteeField(TypeBuilder typeBuilder)
        {
            FieldBuilder adapteeField = typeBuilder.DefineField("adaptee", this.Adaptee, FieldAttributes.Private | FieldAttributes.InitOnly);

            return adapteeField;
        }

        private void GenerateConstructor(TypeBuilder typeBuilder, FieldBuilder adapteeField)
        {
            ConstructorBuilder constructor =
                typeBuilder.DefineConstructor(
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                    CallingConventions.Standard,
                    new[] { this.adaptee });

            constructor.DefineParameter(1, ParameterAttributes.None, "adaptee");

            // Define the reflection ConstructorInfo for System.Object
            ConstructorInfo ci = typeof(object).GetConstructor(new Type[0]);

            // call constructor of base object
            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, ci);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, adapteeField);
            il.Emit(OpCodes.Ret);
        }

        private void GenerateDelegatingMethods(TypeBuilder typeBuilder, FieldBuilder adapteeField)
        {
            var methods = this.Contract.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();

                Type[] parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

                MethodInfo targetMethodOnAdaptee = this.Adaptee.GetMethod(method.Name, parameterTypes);

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                    method.Name,
                    Constants.Attributes.Method,
                    CallingConventions.HasThis,
                    method.ReturnType,
                    parameterTypes);

                for (int i = 0; i < parameters.Length; i++)
                {
                    methodBuilder.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Name);
                }

                ILGenerator il = methodBuilder.GetILGenerator();

                if (method.ReturnType != typeof(void))
                {
                    il.DeclareLocal(method.ReturnType);
                }

                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, adapteeField);

                for (int i = 1; i <= parameters.Length; i++)
                {
                    il.Emit(OpCodes.Ldarg, i);
                }

                il.Emit(OpCodes.Callvirt, targetMethodOnAdaptee);

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

        private void GenerateDelegatingProperties(TypeBuilder typeBuilder, FieldBuilder adapteeField)
        {
            PropertyInfo[] properties = this.Contract.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo property in properties)
            {
                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                    property.Name,
                    PropertyAttributes.None,
                    property.PropertyType,
                    Type.EmptyTypes);

                PropertyInfo targetPropertyOnAdaptee = this.Adaptee.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public);

                MethodBuilder getter = this.GenerateGetMethod(typeBuilder, adapteeField, property, targetPropertyOnAdaptee);

                propertyBuilder.SetGetMethod(getter);

                MethodBuilder setMethod = this.GenerateSetMethod(typeBuilder, adapteeField, property, targetPropertyOnAdaptee);

                propertyBuilder.SetSetMethod(setMethod);
            }
        }

        private MethodBuilder GenerateGetMethod(TypeBuilder typeBuilder, FieldBuilder adapteeField, PropertyInfo property, PropertyInfo targetPropertyOnAdaptee)
        {
            string name = "get_" + property.Name;

            MethodBuilder getMethod = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.GetSetFromInterface,
                property.PropertyType,
                Type.EmptyTypes);

            ILGenerator il = getMethod.GetILGenerator();

            Label ret = il.DefineLabel();
            il.DeclareLocal(property.PropertyType);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, adapteeField);

            il.Emit(OpCodes.Callvirt, targetPropertyOnAdaptee.GetGetMethod());
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br_S, ret);
            il.MarkLabel(ret);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            return getMethod;
        }

        private MethodBuilder GenerateSetMethod(TypeBuilder typeBuilder, FieldBuilder adapteeField, PropertyInfo property, PropertyInfo targetPropertyOnAdaptee)
        {
            string name = "set_" + property.Name;

            MethodBuilder setMethod = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.GetSetFromInterface,
                null,
                new[] { property.PropertyType });

            ILGenerator il = setMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, adapteeField);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, targetPropertyOnAdaptee.SetMethod);
            il.Emit(OpCodes.Ret);

            return setMethod;
        }
    }
}