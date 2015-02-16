namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public class AdapterProxyBuilder : ProxyBuilder
    {
        public AdapterProxyBuilder(ModuleBuilder moduleBuilder, Type contract, Type target)
            : base(moduleBuilder, contract, target)
        {
        }
        
        protected override string GetTypeName()
        {
            return string.Format("{0}To{1}Adapter", this.Target.Name, this.Contract.Name);
        }

        protected override FieldBuilder GenerateTargetField(TypeBuilder typeBuilder)
        {
            FieldBuilder adapteeField = typeBuilder.DefineField("target", this.Target, Constants.Attributes.ReadonlyField);

            return adapteeField;
        }

        protected override void GenerateConstructor(TypeBuilder typeBuilder, FieldBuilder targetField)
        {
            ConstructorBuilder constructor = typeBuilder.DefineConstructor(
                Constants.Attributes.Ctor,
                CallingConventions.Standard,
                new[] { this.Target });

            constructor.DefineParameter(1, ParameterAttributes.None, Constants.Names.TargetField);

            // call the parameterless constructor of the base class (must be done explicitely otherwise the IL code won't be valid)
            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, Constants.Constructors.Object);

            // store the target parameter in the matching private field
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, targetField);
            il.Emit(OpCodes.Ret);
        }

        protected override MethodBuilder GenerateTargetProperty(TypeBuilder typeBuilder, FieldBuilder targetField)
        {
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                Constants.Names.TargetProperty,
                PropertyAttributes.None,
                this.Target,
                Type.EmptyTypes);

            MethodBuilder targetGetter = typeBuilder.DefineMethod(
                Constants.Names.GetterPrefix + Constants.Names.TargetProperty,
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