namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public class DecoraptorProxyBuilder : ProxyBuilder<BuilderContext>
    {
        private const string DecoraptorPostfix = "Decoraptor";

        private const string CreateFieldName = "create";

        private const string CreatePropertyName = "Create";

        public DecoraptorProxyBuilder(ModuleBuilder moduleBuilder, Type contract)
            : base(moduleBuilder, contract, contract)
        {
        }

        protected override string GetTypeName()
        {
            string name = this.Contract.Name.StartsWith("I", StringComparison.Ordinal)
                ? this.Contract.Name.Substring(1)
                : this.Contract.Name;

            name += DecoraptorPostfix;

            return name;
        }

        protected override FieldBuilder GenerateTargetField(TypeBuilder typeBuilder)
        {
            FieldBuilder targetField = typeBuilder.DefineField(CreateFieldName, typeof(Func<>).MakeGenericType(this.Target), Constants.Attributes.ReadonlyField);

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

            ILGenerator il = constructor.GetILGenerator();

            CallParameterlessCtorOfObject(il);

            StoreCtorParameterInField(il, 1, ctx.TargetField);

            il.Emit(OpCodes.Ret);
        }

        protected override MethodBuilder GenerateTargetProperty(TypeBuilder typeBuilder, FieldBuilder targetField)
        {
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                CreatePropertyName,
                PropertyAttributes.None,
                this.Target,
                Type.EmptyTypes);

            MethodBuilder targetGetter = typeBuilder.DefineMethod(
                Constants.Names.GetterPrefix + CreatePropertyName,
                Constants.Attributes.GetSet,
                this.Target,
                Type.EmptyTypes);

            ILGenerator il = targetGetter.GetILGenerator();

            il.DeclareLocal(this.Target);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, targetField);

            MethodInfo invoke = typeof(Func<>).MakeGenericType(this.Target).GetMethod("Invoke");

            il.Emit(OpCodes.Callvirt, invoke);

            PutPropertyValueOnStackAndReturn(il);

            propertyBuilder.SetGetMethod(targetGetter);

            return targetGetter;
        }
    }
}
