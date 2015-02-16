namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    public class DecomissioningDecoraptorProxyBuilder : ProxyBuilder<DecomissioningDecoraptorBuilderContext>
    {
        private const string DecomissioningDecoraptorPostfix = "DecomissioningDecoraptor";

        private const string CreateFieldName = "create";

        private const string CreatePropertyName = "Create";

        private const string ReleaseFieldName = "release";

        private const string ReleaseMethodName = "Release";

        public DecomissioningDecoraptorProxyBuilder(ModuleBuilder moduleBuilder, Type contract)
            : base(moduleBuilder, contract, contract)
        {
        }

        public override Type Build()
        {
            TypeBuilder typeBuilder = this.CreateTypeBuilder();

            FieldBuilder targetField = this.GenerateTargetField(typeBuilder);

            MethodBuilder targetGetter = this.GenerateTargetProperty(typeBuilder, targetField);

            FieldBuilder releaseField = this.GenerateReleaseField(typeBuilder);

            MethodBuilder releaseMethod = this.GenerateReleaseMethod(typeBuilder, releaseField);

            DecomissioningDecoraptorBuilderContext ctx = this.CreateContext();

            ctx.TypeBuilder = typeBuilder;
            ctx.TargetField = targetField;
            ctx.TargetGetter = targetGetter;
            ctx.ReleaseField = releaseField;
            ctx.ReleaseMethod = releaseMethod;

            this.GenerateConstructor(ctx);

            this.GenerateDelegatingProperties(ctx);

            this.GenerateDelegatingMethods(ctx);

            Type adapterType = typeBuilder.CreateType();

            return adapterType;
        }

        protected override string GetTypeName()
        {
            string name = this.Contract.Name.StartsWith("I", StringComparison.Ordinal)
                ? this.Contract.Name.Substring(1)
                : this.Contract.Name;

            name += DecomissioningDecoraptorPostfix;

            return name;
        }

        protected override FieldBuilder GenerateTargetField(TypeBuilder typeBuilder)
        {
            FieldBuilder targetField = typeBuilder.DefineField(CreateFieldName, typeof(Func<>).MakeGenericType(this.Target), Constants.Attributes.ReadonlyField);

            return targetField;
        }

        private FieldBuilder GenerateReleaseField(TypeBuilder typeBuilder)
        {
            FieldBuilder releaseField = typeBuilder.DefineField(ReleaseFieldName, typeof(Action<>).MakeGenericType(this.Target), Constants.Attributes.ReadonlyField);

            return releaseField;
        }

        private MethodBuilder GenerateReleaseMethod(TypeBuilder typeBuilder, FieldBuilder releaseField)
        {
            MethodBuilder release = typeBuilder.DefineMethod(
                ReleaseMethodName, 
                Constants.Attributes.Method, 
                null,
                new[] {this.Target});

            ILGenerator il = release.GetILGenerator();

            Label ret = il.DefineLabel();
            il.DeclareLocal(typeof (bool));
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Brtrue, ret);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, releaseField);
            il.Emit(OpCodes.Ldarg_1);
            MethodInfo invoke = typeof (Action<>).MakeGenericType(this.Target).GetMethod("Invoke", new[] {this.Target});
            il.Emit(OpCodes.Callvirt, invoke);

            il.MarkLabel(ret);
            il.Emit(OpCodes.Ret);

            return release;
        }

        protected override void GenerateConstructor(DecomissioningDecoraptorBuilderContext ctx)
        {
            Type create = typeof(Func<>).MakeGenericType(this.Target);
            Type release = typeof(Action<>).MakeGenericType(this.Target);

            ConstructorBuilder constructor = ctx.TypeBuilder.DefineConstructor(
                Constants.Attributes.Ctor,
                CallingConventions.Standard,
                new[] { create, release });

            constructor.DefineParameter(1, ParameterAttributes.None, "create");
            constructor.DefineParameter(2, ParameterAttributes.None, "release");

            ILGenerator il = constructor.GetILGenerator();

            CallParameterlessCtorOfObject(il);

            StoreCtorParameterInField(il, 1, ctx.TargetField);
            StoreCtorParameterInField(il, 2, ctx.ReleaseField);

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

        protected override void CallMethodOnTarget(ILGenerator il, DecomissioningDecoraptorBuilderContext ctx, MethodInfo methodOnTarget, MethodInfo methodOnContract, ICollection<ParameterInfo> parameters)
        {
            il.DeclareLocal(this.Target);

            if (MethodHasReturnValue(methodOnContract))
            {
                il.DeclareLocal(methodOnContract.ReturnType);
            }

            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stloc_0);
            
            il.BeginExceptionBlock();

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, ctx.TargetGetter);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);

            // load all method parameters so you can hand them down to the method of the target
            for (int i = 1; i <= parameters.Count; i++)
            {
                il.Emit(OpCodes.Ldarg, i);
            }

            // call the method on the target
            il.Emit(OpCodes.Callvirt, methodOnTarget);

            if (MethodHasReturnValue(methodOnContract))
            {
                il.Emit(OpCodes.Stloc_1);
            }

            il.BeginFinallyBlock();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, ctx.ReleaseMethod);

            il.EndExceptionBlock();

            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ret);
        }

        protected override MethodBuilder GenerateGetMethod(DecomissioningDecoraptorBuilderContext ctx, MethodInfo getterOnContract, MethodInfo getterOnTarget, Type propertyType)
        {
            MethodBuilder getMethod = this.DefineGetMethod(ctx.TypeBuilder, getterOnContract, propertyType);

            ILGenerator il = getMethod.GetILGenerator();

            il.DeclareLocal(this.Target);
            il.DeclareLocal(propertyType);

            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stloc_0);

            il.BeginExceptionBlock();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, ctx.TargetGetter);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);

            il.Emit(OpCodes.Callvirt, getterOnTarget);

            il.Emit(OpCodes.Stloc_1);
            il.BeginFinallyBlock();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, ctx.ReleaseMethod);

            il.EndExceptionBlock();

            // load the result value on the stack. will be used as the return value when the method returns
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ret);

            return getMethod;
        }

        protected override MethodBuilder GenerateSetMethod(DecomissioningDecoraptorBuilderContext ctx, MethodInfo setterOnContract, MethodInfo setterOnTarget, Type propertyType)
        {
            MethodBuilder setMethod = this.DefineSetMethod(ctx.TypeBuilder, setterOnContract, propertyType);

            ILGenerator il = setMethod.GetILGenerator();

            il.DeclareLocal(this.Target);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stloc_0);

            il.BeginExceptionBlock();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, ctx.TargetGetter);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, setterOnTarget);

            il.BeginFinallyBlock();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, ctx.ReleaseMethod);
            il.EndExceptionBlock();

            il.Emit(OpCodes.Ret);

            return setMethod;
        }
    }
}