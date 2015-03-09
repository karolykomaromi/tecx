namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public class LazyProxyBuilder : ProxyBuilder<BuilderContext>
    {
        private const string LazyPrefix = "Lazy";

        private const string InstanceFieldName = "instance";

        private const string InstancePropertyName = "Instance";

        public LazyProxyBuilder(ModuleBuilder moduleBuilder, Type contract)
            : base(moduleBuilder, contract, contract)
        {
            System.Diagnostics.Contracts.Contract.Requires(moduleBuilder != null);
            System.Diagnostics.Contracts.Contract.Requires(contract != null);
        }

        protected override string GetTypeName()
        {
            string name = LazyPrefix;

            if (this.Contract.Name.StartsWith("I", StringComparison.Ordinal))
            {
                name += this.Contract.Name.Substring(1);
            }
            else
            {
                name += this.Contract.Name;
            }

            return name;
        }

        protected override FieldBuilder GenerateTargetField(TypeBuilder typeBuilder)
        {
            FieldBuilder targetField = typeBuilder.DefineField(InstanceFieldName, typeof(Lazy<>).MakeGenericType(this.Target), Constants.Attributes.ReadonlyField);

            return targetField;
        }

        protected override void GenerateConstructor(BuilderContext ctx)
        {
            Type factory = typeof(Func<>).MakeGenericType(this.Target);

            ConstructorBuilder constructor = ctx.TypeBuilder.DefineConstructor(
                Constants.Attributes.Ctor,
                CallingConventions.Standard,
                new[] { factory });

            constructor.DefineParameter(1, ParameterAttributes.None, "factory");

            // call the parameterless constructor of the base class (must be done explicitely otherwise the IL code won't be valid)
            ILGenerator il = constructor.GetILGenerator();

            LazyProxyBuilder.CallParameterlessCtorOfObject(il);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);

            // new Lazy<T>(Func<T>)
            ConstructorInfo lazyCtor = typeof(Lazy<>).MakeGenericType(this.Target).GetConstructor(new[] { factory });
            il.Emit(OpCodes.Newobj, lazyCtor);

            il.Emit(OpCodes.Stfld, ctx.TargetField);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }

        protected override MethodBuilder GenerateTargetProperty(TypeBuilder typeBuilder, FieldBuilder targetField)
        {
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                InstancePropertyName,
                PropertyAttributes.None,
                this.Target,
                Type.EmptyTypes);

            MethodBuilder targetGetter = typeBuilder.DefineMethod(
                Constants.Names.GetterPrefix + InstancePropertyName,
                Constants.Attributes.GetSet,
                this.Target,
                Type.EmptyTypes);

            ILGenerator il = targetGetter.GetILGenerator();

            il.DeclareLocal(this.Target);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, targetField);

            MethodInfo valueGetter = typeof(Lazy<>).MakeGenericType(this.Target).GetProperty("Value").GetGetMethod();

            il.Emit(OpCodes.Callvirt, valueGetter);

            LazyProxyBuilder.PutPropertyValueOnStackAndReturn(il);

            propertyBuilder.SetGetMethod(targetGetter);

            return targetGetter;
        }
    }
}
