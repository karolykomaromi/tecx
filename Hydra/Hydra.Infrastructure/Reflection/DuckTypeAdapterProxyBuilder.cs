namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public class DuckTypeAdapterProxyBuilder : ProxyBuilder<BuilderContext>
    {
        private const string TargetFieldName = "target";

        private const string TargetPropertyName = "Target";

        public DuckTypeAdapterProxyBuilder(ModuleBuilder moduleBuilder, Type contract, Type target)
            : base(moduleBuilder, contract, target)
        {
            System.Diagnostics.Contracts.Contract.Requires(moduleBuilder != null);
            System.Diagnostics.Contracts.Contract.Requires(contract != null);
            System.Diagnostics.Contracts.Contract.Requires(target != null);
        }
        
        protected override string GetTypeName()
        {
            return string.Format("{0}To{1}Adapter", this.Target.Name, this.Contract.Name);
        }

        protected override FieldBuilder GenerateTargetField(TypeBuilder typeBuilder)
        {
            FieldBuilder targetField = typeBuilder.DefineField(TargetFieldName, this.Target, Constants.Attributes.ReadonlyField);

            return targetField;
        }

        protected override void GenerateConstructor(BuilderContext ctx)
        {
            ConstructorBuilder constructor = ctx.TypeBuilder.DefineConstructor(
                Constants.Attributes.Ctor,
                CallingConventions.Standard,
                new[] { this.Target });

            constructor.DefineParameter(1, ParameterAttributes.None, TargetFieldName);

            // call the parameterless constructor of the base class (must be done explicitely otherwise the IL code won't be valid)
            ILGenerator il = constructor.GetILGenerator();

            DuckTypeAdapterProxyBuilder.CallParameterlessCtorOfObject(il);

            DuckTypeAdapterProxyBuilder.StoreCtorParameterInField(il, 1, ctx.TargetField);

            il.Emit(OpCodes.Ret);
        }

        protected override MethodBuilder GenerateTargetProperty(TypeBuilder typeBuilder, FieldBuilder targetField)
        {
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                TargetPropertyName,
                PropertyAttributes.None,
                this.Target,
                Type.EmptyTypes);

            MethodBuilder targetGetter = typeBuilder.DefineMethod(
                Constants.Names.GetterPrefix + TargetPropertyName,
                Constants.Attributes.GetSet,
                this.Target,
                Type.EmptyTypes);

            ILGenerator il = targetGetter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, targetField);
            il.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(targetGetter);

            return targetGetter;
        }
    }
}