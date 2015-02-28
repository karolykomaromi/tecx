namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;

    public class Method
    {
        public static readonly MethodInfo Null = new NullMethodInfo();

        private class NullMethodInfo : MethodInfo
        {
            public override ICustomAttributeProvider ReturnTypeCustomAttributes
            {
                get { return CustomAttributeProvider.Null; }
            }

            public override string Name
            {
                get { return string.Empty; }
            }

            public override Type DeclaringType
            {
                get { return typeof(object); }
            }

            public override Type ReflectedType
            {
                get { return typeof(object); }
            }

            public override RuntimeMethodHandle MethodHandle
            {
                get { return new RuntimeMethodHandle(); }
            }

            public override MethodAttributes Attributes
            {
                get { return (MethodAttributes)0; }
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                return new object[0];
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                return false;
            }

            public override ParameterInfo[] GetParameters()
            {
                return new ParameterInfo[0];
            }

            public override MethodImplAttributes GetMethodImplementationFlags()
            {
                return (MethodImplAttributes)0;
            }

            [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Implementing external API.")]
            public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
            {
                return new object();
            }

            public override MethodInfo GetBaseDefinition()
            {
                return this;
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return new object[0];
            }
        }
    }
}