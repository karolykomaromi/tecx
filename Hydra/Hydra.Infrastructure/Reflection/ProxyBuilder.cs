namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public abstract class ProxyBuilder : Builder<Type>
    {
        private readonly ModuleBuilder moduleBuilder;

        private readonly Type contract;

        private readonly Type target;

        protected ProxyBuilder(ModuleBuilder moduleBuilder, Type contract, Type target)
        {
            System.Diagnostics.Contracts.Contract.Requires(moduleBuilder != null);
            System.Diagnostics.Contracts.Contract.Requires(contract != null);
            System.Diagnostics.Contracts.Contract.Requires(contract.IsInterface);
            System.Diagnostics.Contracts.Contract.Requires(target != null);

            this.moduleBuilder = moduleBuilder;
            this.contract = contract;
            this.target = target;
        }

        protected ModuleBuilder ModuleBuilder
        {
            get { return this.moduleBuilder; }
        }

        protected Type Contract
        {
            get { return this.contract; }
        }

        protected Type Target
        {
            get { return this.target; }
        }

        public override Type Build()
        {
            TypeBuilder typeBuilder = this.CreateTypeBuilder();

            FieldBuilder targetField = this.GenerateTargetField(typeBuilder);

            this.GenerateConstructor(typeBuilder, targetField);

            MethodBuilder targetGetter = this.GenerateTargetProperty(typeBuilder, targetField);

            this.GenerateDelegatingProperties(typeBuilder, targetGetter);

            this.GenerateDelegatingMethods(typeBuilder, targetGetter);

            Type adapterType = typeBuilder.CreateType();

            return adapterType;
        }

        protected static void CallParameterlessCtorOfObject(ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, Constants.Constructors.Object);
        }

        protected static void StoreCtorParameterInField(ILGenerator il, FieldBuilder targetField)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, targetField);
            il.Emit(OpCodes.Ret);
        }

        protected static void PutPropertyValueOnStackAndReturn(ILGenerator il)
        {
            Label ret = il.DefineLabel();
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br_S, ret);
            il.MarkLabel(ret);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
        }

        protected virtual TypeBuilder CreateTypeBuilder()
        {
            string name = this.GetTypeName();

            TypeBuilder typeBuilder = this.ModuleBuilder.DefineType(
                name,
                Constants.Attributes.GeneratedType,
                typeof(object),
                new[] { this.Contract });

            return typeBuilder;
        }

        protected abstract string GetTypeName();

        protected abstract FieldBuilder GenerateTargetField(TypeBuilder typeBuilder);

        protected abstract void GenerateConstructor(TypeBuilder typeBuilder, FieldBuilder targetField);

        protected abstract MethodBuilder GenerateTargetProperty(TypeBuilder typeBuilder, FieldBuilder targetField);

        protected virtual MethodBuilder DefineMethod(TypeBuilder typeBuilder, MethodInfo methodOnContract, Type[] parameterTypes)
        {
            string name = this.GetMethodName(methodOnContract);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.ExplicitMethod,
                CallingConventions.HasThis,
                methodOnContract.ReturnType,
                parameterTypes);
            return methodBuilder;
        }

        protected virtual string GetMethodName(MethodInfo methodOnContract)
        {
            return this.Contract.FullName + "." + methodOnContract.Name;
        }

        protected virtual MethodBuilder DefineGetMethod(TypeBuilder typeBuilder, MethodInfo getterOnContract, Type propertyType)
        {
            string name = this.GetGetMethodName(getterOnContract);

            MethodBuilder getMethod = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.ExplicitGetSetFromInterface,
                propertyType,
                Type.EmptyTypes);

            return getMethod;
        }

        protected virtual string GetGetMethodName(MethodInfo getterOnContract)
        {
            return this.Contract.FullName + "." + getterOnContract.Name;
        }

        protected virtual MethodBuilder DefineSetMethod(TypeBuilder typeBuilder, MethodInfo setterOnContract, Type propertyType)
        {
            string name = this.GetSetMethodName(setterOnContract);

            MethodBuilder setMethod = typeBuilder.DefineMethod(
                name,
                Constants.Attributes.ExplicitGetSetFromInterface,
                null,
                new[] { propertyType });

            return setMethod;
        }

        protected virtual string GetSetMethodName(MethodInfo setterOnContract)
        {
            return this.Contract.FullName + "." + setterOnContract.Name;
        }

        protected virtual void GenerateDelegatingMethods(TypeBuilder typeBuilder, MethodBuilder targetGetter)
        {
            var methods = this.Contract.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(MethodIsNeitherGetterNorSetter)
                .OrderBy(m => m.Name, StringComparer.Ordinal);

            foreach (MethodInfo methodOnContract in methods)
            {
                ParameterInfo[] parameters = methodOnContract.GetParameters();

                Type[] parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

                MethodBuilder methodBuilder = this.DefineMethod(typeBuilder, methodOnContract, parameterTypes);

                for (int i = 0; i < parameters.Length; i++)
                {
                    methodBuilder.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Name);
                }

                MethodInfo targetMethodOnAdaptee = this.Target.GetMethod(methodOnContract.Name, parameterTypes);

                ILGenerator il = methodBuilder.GetILGenerator();

                if (MemberDoesNotExist(targetMethodOnAdaptee))
                {
                    ThrowNotImplementedException(il);
                }
                else
                {
                    CallMethodOnTarget(il, targetGetter, targetMethodOnAdaptee, methodOnContract, parameters);
                }

                typeBuilder.DefineMethodOverride(methodBuilder, methodOnContract);
            }
        }

        protected virtual void GenerateDelegatingProperties(TypeBuilder typeBuilder, MethodBuilder targetGetter)
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

                PropertyInfo propertyOnAdaptee = this.Target.GetProperty(propertyOnContract.Name, BindingFlags.Instance | BindingFlags.Public);

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

                MethodBuilder getter = this.GenerateGetMethod(typeBuilder, targetGetter, getterOnContract, getterOnTarget, propertyType);

                propertyBuilder.SetGetMethod(getter);

                typeBuilder.DefineMethodOverride(getter, getterOnContract);

                if (PropertyIsPubliclyWritable(setterOnContract))
                {
                    MethodBuilder setter = this.GenerateSetMethod(typeBuilder, targetGetter, setterOnContract, setterOnTarget, propertyType);

                    propertyBuilder.SetSetMethod(setter);

                    typeBuilder.DefineMethodOverride(setter, setterOnContract);
                }
            }
        }

        protected virtual MethodBuilder GenerateGetMethod(TypeBuilder typeBuilder, MethodBuilder targetGetter, MethodInfo getterOnContract, MethodInfo getterOnTarget, Type propertyType)
        {
            MethodBuilder getMethod = this.DefineGetMethod(typeBuilder, getterOnContract, propertyType);

            ILGenerator il = getMethod.GetILGenerator();

            Label ret = il.DefineLabel();

            // prepare a field to store the return value
            il.DeclareLocal(propertyType);

            // access the field with the target instance
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetGetter);

            // call the property on the target
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

        protected virtual MethodBuilder GenerateNotImplementedGetMethod(TypeBuilder typeBuilder, MethodInfo getterOnContract, Type propertyType)
        {
            MethodBuilder getMethod = this.DefineGetMethod(typeBuilder, getterOnContract, propertyType);

            ILGenerator il = getMethod.GetILGenerator();

            ThrowNotImplementedException(il);

            return getMethod;
        }

        protected virtual MethodBuilder GenerateSetMethod(TypeBuilder typeBuilder, MethodBuilder targetGetter, MethodInfo setterOnContract, MethodInfo setterOnTarget, Type propertyType)
        {
            MethodBuilder setMethod = this.DefineSetMethod(typeBuilder, setterOnContract, propertyType);

            ILGenerator il = setMethod.GetILGenerator();

            // access the field with the target instance
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetGetter);

            // store the value using the setter of the target property
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, setterOnTarget);
            il.Emit(OpCodes.Ret);

            return setMethod;
        }

        protected virtual MethodBuilder GenerateNotImplementedSetMethod(TypeBuilder typeBuilder, MethodInfo setterOnContract, Type propertyType)
        {
            MethodBuilder setMethod = this.DefineSetMethod(typeBuilder, setterOnContract, propertyType);

            ILGenerator il = setMethod.GetILGenerator();

            ThrowNotImplementedException(il);

            return setMethod;
        }
        
        private static bool MethodHasReturnValue(MethodInfo method)
        {
            return method.ReturnType != typeof(void);
        }

        private static void CallMethodOnTarget(ILGenerator il, MethodBuilder targetGetter, MethodInfo targetMethodOnAdaptee, MethodInfo methodOnContract, ICollection<ParameterInfo> parameters)
        {
            if (MethodHasReturnValue(methodOnContract))
            {
                // prepare a field to store the return value
                il.DeclareLocal(methodOnContract.ReturnType);
            }

            // access the field with the target instance
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, targetGetter);

            // load all method parameters so you can hand them down to the method of the target
            for (int i = 1; i <= parameters.Count; i++)
            {
                il.Emit(OpCodes.Ldarg, i);
            }

            // call the method on the target
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
    }
}