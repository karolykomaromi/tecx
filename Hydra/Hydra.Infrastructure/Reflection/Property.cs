namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class Property
    {
        public static readonly PropertyInfo Null = new NullPropertyInfo();

        public static PropertyInfo Get<TProperty>(Expression<Func<TProperty>> propertySelector)
        {
            MemberExpression property = propertySelector.Body as MemberExpression;

            PropertyInfo info;
            if (property != null && (info = property.Member as PropertyInfo) != null)
            {
                return info;
            }

            return Property.Null;
        }

        public static PropertyInfo Get<T, TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            MemberExpression property = propertySelector.Body as MemberExpression;

            PropertyInfo info;
            if (property != null && (info = property.Member as PropertyInfo) != null)
            {
                return info;
            }

            return Property.Null;
        }

        private class NullPropertyInfo : PropertyInfo
        {
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

            public override Type PropertyType
            {
                get { return typeof(Missing); }
            }

            public override PropertyAttributes Attributes
            {
                get { return (PropertyAttributes)0; }
            }

            public override bool CanRead
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                return new object[0];
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                return false;
            }

            public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
            {
                return new object();
            }

            public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
            {
            }

            public override MethodInfo[] GetAccessors(bool nonPublic)
            {
                return new MethodInfo[0];
            }

            public override MethodInfo GetGetMethod(bool nonPublic)
            {
                return Method.Null;
            }

            public override MethodInfo GetSetMethod(bool nonPublic)
            {
                return Method.Null;
            }

            public override ParameterInfo[] GetIndexParameters()
            {
                return new ParameterInfo[0];
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return new object[0];
            }
        }
    }
}