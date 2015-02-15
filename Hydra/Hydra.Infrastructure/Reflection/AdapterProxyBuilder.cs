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

        private static bool MethodIsNeitherGetterNorSetter(MethodInfo method)
        {
            return !method.Name.StartsWith(Constants.Names.GetterPrefix, StringComparison.OrdinalIgnoreCase) &&
                !method.Name.StartsWith(Constants.Names.SetterPrefix, StringComparison.OrdinalIgnoreCase);
        }

        private static bool PropertyIsPubliclyWritable(MethodInfo setterOnContract)
        {
            return setterOnContract != null;
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
            var methods = this.Contract.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(MethodIsNeitherGetterNorSetter)
                .OrderBy(m => m.Name, StringComparer.Ordinal);

            foreach (MethodInfo methodOnContract in methods)
            {
                ParameterInfo[] parameters = methodOnContract.GetParameters();

                Type[] parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

                string name = this.GetMethodName(methodOnContract);

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                    name,
                    Constants.Attributes.ExplicitMethod,
                    CallingConventions.HasThis,
                    methodOnContract.ReturnType,
                    parameterTypes);

                for (int i = 0; i < parameters.Length; i++)
                {
                    methodBuilder.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Name);
                }

                MethodInfo targetMethodOnAdaptee = this.Adaptee.GetMethod(methodOnContract.Name, parameterTypes);

                ILGenerator il = methodBuilder.GetILGenerator();

                if (MemberDoesNotExist(targetMethodOnAdaptee))
                {
                    ThrowNotImplementedException(il);

                    typeBuilder.DefineMethodOverride(methodBuilder, methodOnContract);

                    continue;
                }

                if (MethodHasReturnValue(methodOnContract))
                {
                    // prepare a field to store the return value
                    il.DeclareLocal(methodOnContract.ReturnType);
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

                if (MethodHasReturnValue(methodOnContract))
                {
                    // store the result of the method call to the prepared field
                    // and jump to the end of the method
                    il.Emit(OpCodes.Stloc_0);
                    Label ret = il.DefineLabel();
                    il.Emit(OpCodes.Br_S, ret);
                    il.MarkLabel(ret);

                    // load the result value on the stack. will be used as the return value when the method returns
                    il.Emit(OpCodes.Ldloc_0);
                }

                // end of method. any result stored at this point will be returned
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ret);

                typeBuilder.DefineMethodOverride(methodBuilder, methodOnContract);
            }
        }

        private void GenerateDelegatingProperties(TypeBuilder typeBuilder, FieldBuilder targetField)
        {
            var properties = this.Contract.GetProperties(BindingFlags.Instance | BindingFlags.Public).OrderBy(p => p.Name, StringComparer.Ordinal);

            foreach (PropertyInfo propertyOnContract in properties)
            {
                Type propertyType = propertyOnContract.PropertyType;

                MethodInfo getterOnContract = propertyOnContract.GetGetMethod();

                MethodInfo setterOnContract = propertyOnContract.GetSetMethod();

                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                    propertyOnContract.Name,
                    PropertyAttributes.None,
                    propertyType,
                    Type.EmptyTypes);

                PropertyInfo propertyOnAdaptee = this.Adaptee.GetProperty(propertyOnContract.Name, BindingFlags.Instance | BindingFlags.Public);

                if (MemberDoesNotExist(propertyOnAdaptee))
                {
                    MethodBuilder notImplementedGetter = this.GenerateNotImplementedGetMethod(typeBuilder, getterOnContract, propertyType);

                    propertyBuilder.SetGetMethod(notImplementedGetter);

                    typeBuilder.DefineMethodOverride(notImplementedGetter, getterOnContract);

                    if (PropertyIsPubliclyWritable(setterOnContract))
                    {
                        MethodBuilder notImplementedSetter = this.GenerateNotImplementedSetMethod(typeBuilder, setterOnContract, propertyType);

                        propertyBuilder.SetSetMethod(notImplementedSetter);

                        typeBuilder.DefineMethodOverride(notImplementedSetter, setterOnContract);
                    }

                    continue;
                }

                MethodInfo getterOnTarget = propertyOnAdaptee.GetGetMethod();

                MethodInfo setterOnTarget = propertyOnAdaptee.GetSetMethod();

                MethodBuilder getter = this.GenerateGetMethod(typeBuilder, targetField, getterOnContract, getterOnTarget, propertyType);

                propertyBuilder.SetGetMethod(getter);

                typeBuilder.DefineMethodOverride(getter, getterOnContract);

                if (PropertyIsPubliclyWritable(setterOnContract))
                {
                    MethodBuilder setter = this.GenerateSetMethod(typeBuilder, targetField, setterOnContract, setterOnTarget, propertyType);

                    propertyBuilder.SetSetMethod(setter);

                    typeBuilder.DefineMethodOverride(setter, setterOnContract);
                }
            }
        }

        private MethodBuilder GenerateGetMethod(TypeBuilder typeBuilder, FieldBuilder targetField, MethodInfo getterOnContract, MethodInfo getterOnTarget, Type propertyType)
        {
            MethodBuilder getMethod = this.DefineGetMethod(typeBuilder, getterOnContract, propertyType);

            ILGenerator il = getMethod.GetILGenerator();

            Label ret = il.DefineLabel();

            // prepare a field to store the return value
            il.DeclareLocal(propertyType);

            // access the field with the adaptee instance
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, targetField);

            // call the property on the adaptee
            il.Emit(OpCodes.Callvirt, getterOnTarget);

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

        private MethodBuilder GenerateNotImplementedGetMethod(TypeBuilder typeBuilder, MethodInfo getterOnContract, Type propertyType)
        {
            MethodBuilder getMethod = this.DefineGetMethod(typeBuilder, getterOnContract, propertyType);

            ILGenerator il = getMethod.GetILGenerator();

            ThrowNotImplementedException(il);

            return getMethod;
        }

        private MethodBuilder GenerateSetMethod(TypeBuilder typeBuilder, FieldBuilder targetField, MethodInfo setterOnContract, MethodInfo setterOnTarget, Type propertyType)
        {
            MethodBuilder setMethod = this.DefineSetMethod(typeBuilder, setterOnContract, propertyType);

            ILGenerator il = setMethod.GetILGenerator();

            // access the field with the adaptee instance
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, targetField);

            // store the value using the setter of the adaptee property
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, setterOnTarget);
            il.Emit(OpCodes.Ret);

            return setMethod;
        }

        private MethodBuilder GenerateNotImplementedSetMethod(TypeBuilder typeBuilder, MethodInfo setterOnContract, Type propertyType)
        {
            MethodBuilder setMethod = DefineSetMethod(typeBuilder, setterOnContract, propertyType);

            ILGenerator il = setMethod.GetILGenerator();

            ThrowNotImplementedException(il);

            return setMethod;
        }
        
        private MethodBuilder DefineGetMethod(TypeBuilder typeBuilder, MethodInfo getterOnContract, Type propertyType)
        {
            string name = this.GetGetMethodName(getterOnContract);

            MethodBuilder getMethod = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.GetSetFromInterface,
                propertyType,
                Type.EmptyTypes);

            return getMethod;
        }

        private MethodBuilder DefineSetMethod(TypeBuilder typeBuilder, MethodInfo setterOnContract, Type propertyType)
        {
            string name = this.GetSetMethodName(setterOnContract);

            MethodBuilder setMethod = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.GetSetFromInterface,
                null,
                new[] { propertyType });

            return setMethod;
        }

    }
}