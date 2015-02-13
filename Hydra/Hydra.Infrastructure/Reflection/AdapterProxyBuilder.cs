namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public class AdapterProxyBuilder : ProxyBuilder
    {
        private readonly Type adaptee;

        public AdapterProxyBuilder(ModuleBuilder moduleBuilder, Type contract, Type adaptee)
            : base(moduleBuilder, contract)
        {
            System.Diagnostics.Contracts.Contract.Requires(adaptee != null);

            this.adaptee = adaptee;
        }

        private Type Adaptee
        {
            get { return this.adaptee; }
        }

        public override Type Build()
        {
            TypeBuilder typeBuilder = this.CreateTypeBuilder();

            FieldBuilder adapteeField = this.GenerateAdapteeField(typeBuilder);

            this.GenerateConstructor(typeBuilder, adapteeField);

            this.GenerateDelegatingProperties(typeBuilder, adapteeField);

            this.GenerateDelegatingMethods(typeBuilder, adapteeField);

            Type adapterType = typeBuilder.CreateType();

            return adapterType;
        }

        protected override string GetTypeName()
        {
            return string.Format("{0}To{1}Adapter", this.Adaptee.Name, this.Contract.Name);
        }

        private static bool MethodHasReturnValue(MethodInfo method)
        {
            return method.ReturnType != typeof(void);
        }

        private static void ThrowNotImplementedException(ILGenerator il)
        {
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, Constants.Constructors.NotImplementedException);
            il.Emit(OpCodes.Throw);
        }

        private static bool MemberDoesNotExist(MemberInfo member)
        {
            return member == null;
        }

        private static bool PropertyIsPubliclyWritable(PropertyInfo property)
        {
            return property.CanWrite && property.GetSetMethod().IsPublic;
        }

        private static MethodBuilder DefineGetMethod(TypeBuilder typeBuilder, PropertyInfo property)
        {
            string name = Constants.Names.GetterPrefix + property.Name;

            MethodBuilder getMethod = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.GetSetFromInterface,
                property.PropertyType,
                Type.EmptyTypes);
            return getMethod;
        }

        private static MethodBuilder DefineSetMethod(TypeBuilder typeBuilder, PropertyInfo property)
        {
            string name = Constants.Names.SetterPrefix + property.Name;

            MethodBuilder setMethod = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.GetSetFromInterface,
                null,
                new[] { property.PropertyType });
            return setMethod;
        }

        private FieldBuilder GenerateAdapteeField(TypeBuilder typeBuilder)
        {
            FieldBuilder adapteeField = typeBuilder.DefineField("adaptee", this.Adaptee, Constants.Attributes.ReadonlyField);

            return adapteeField;
        }

        private void GenerateConstructor(TypeBuilder typeBuilder, FieldBuilder adapteeField)
        {
            ConstructorBuilder constructor = typeBuilder.DefineConstructor(
                Constants.Attributes.Ctor,
                CallingConventions.Standard,
                new[] { this.adaptee });

            constructor.DefineParameter(1, ParameterAttributes.None, "adaptee");

            // call the parameterless constructor of the base class (must be done explicitely otherwise the IL code won't be valid)
            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, Constants.Constructors.Object);

            // store the adaptee parameter in the matching private field
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

                MethodInfo targetMethodOnAdaptee = this.Adaptee.GetMethod(method.Name, parameterTypes);

                ILGenerator il = methodBuilder.GetILGenerator();

                if (MemberDoesNotExist(targetMethodOnAdaptee))
                {
                    ThrowNotImplementedException(il);

                    continue;
                }

                if (MethodHasReturnValue(method))
                {
                    // prepare a field to store the return value
                    il.DeclareLocal(method.ReturnType);
                }

                // access the field with the adaptee instance
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, adapteeField);

                // load all method parameters so you can hand them down to the method of the adaptee
                for (int i = 1; i <= parameters.Length; i++)
                {
                    il.Emit(OpCodes.Ldarg, i);
                }

                // call the method on the adaptee
                il.Emit(OpCodes.Callvirt, targetMethodOnAdaptee);

                if (MethodHasReturnValue(method))
                {
                    // store the result of the method call to the prepared field
                    // and jump to the end of the method
                    il.Emit(OpCodes.Stloc_0);
                    var local = il.DefineLabel();
                    il.Emit(OpCodes.Br_S, local);
                    il.MarkLabel(local);

                    // load the result value on the stack. will be used as the return value when the method returns
                    il.Emit(OpCodes.Ldloc_0);
                }

                // end of method. any result stored at this point will be returned
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

                if (MemberDoesNotExist(targetPropertyOnAdaptee))
                {
                    MethodBuilder notImplementedGetter = this.GenerateNotImplementedGetMethod(typeBuilder, property);

                    propertyBuilder.SetGetMethod(notImplementedGetter);

                    if (PropertyIsPubliclyWritable(property))
                    {
                        MethodBuilder notImplementedSetMethod = this.GenerateNotImplementedSetMethod(typeBuilder, property);

                        propertyBuilder.SetSetMethod(notImplementedSetMethod);
                    }

                    continue;
                }

                MethodBuilder getter = this.GenerateGetMethod(typeBuilder, adapteeField, property, targetPropertyOnAdaptee);

                propertyBuilder.SetGetMethod(getter);

                if (PropertyIsPubliclyWritable(property))
                {
                    MethodBuilder setMethod = this.GenerateSetMethod(typeBuilder, adapteeField, property, targetPropertyOnAdaptee);

                    propertyBuilder.SetSetMethod(setMethod);
                }
            }
        }

        private MethodBuilder GenerateGetMethod(TypeBuilder typeBuilder, FieldBuilder adapteeField, PropertyInfo property, PropertyInfo targetPropertyOnAdaptee)
        {
            MethodBuilder getMethod = DefineGetMethod(typeBuilder, property);

            ILGenerator il = getMethod.GetILGenerator();

            Label ret = il.DefineLabel();

            // prepare a field to store the return value
            il.DeclareLocal(property.PropertyType);

            // access the field with the adaptee instance
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, adapteeField);

            // call the property on the adaptee
            il.Emit(OpCodes.Callvirt, targetPropertyOnAdaptee.GetGetMethod());

            // store the result of the property call to the prepared field
            // and jump to the end of the property
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br_S, ret);
            il.MarkLabel(ret);

            // load the result value on the stack. will be used as the return value when the method returns
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            return getMethod;
        }

        private MethodBuilder GenerateNotImplementedGetMethod(TypeBuilder typeBuilder, PropertyInfo property)
        {
            MethodBuilder getMethod = DefineGetMethod(typeBuilder, property);

            ILGenerator il = getMethod.GetILGenerator();

            ThrowNotImplementedException(il);

            return getMethod;
        }

        private MethodBuilder GenerateSetMethod(TypeBuilder typeBuilder, FieldBuilder adapteeField, PropertyInfo property, PropertyInfo targetPropertyOnAdaptee)
        {
            MethodBuilder setMethod = DefineSetMethod(typeBuilder, property);

            ILGenerator il = setMethod.GetILGenerator();

            // access the field with the adaptee instance
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, adapteeField);

            // store the value using the setter of the adaptee property
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, targetPropertyOnAdaptee.SetMethod);
            il.Emit(OpCodes.Ret);

            return setMethod;
        }

        private MethodBuilder GenerateNotImplementedSetMethod(TypeBuilder typeBuilder, PropertyInfo property)
        {
            MethodBuilder setMethod = DefineSetMethod(typeBuilder, property);

            ILGenerator il = setMethod.GetILGenerator();

            ThrowNotImplementedException(il);

            return setMethod;
        }
    }
}