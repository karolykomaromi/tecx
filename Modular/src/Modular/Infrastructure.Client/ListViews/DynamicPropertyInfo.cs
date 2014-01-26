namespace Infrastructure.ListViews
{
    using System;
    using System.Globalization;
    using System.Reflection;

    public class DynamicPropertyInfo : PropertyInfo
    {
        private readonly Type propertyType;
        private readonly Type declaringType;
        private readonly string propertyName;

        public DynamicPropertyInfo(Type propertyType, Type declaringType, string propertyName)
        {
            this.propertyType = propertyType;
            this.declaringType = declaringType;
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Could do some System.ComponentModel.DataAnnotation action here if you wanted
        /// </summary>
        public override PropertyAttributes Attributes
        {
            get { return PropertyAttributes.None; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override Type PropertyType
        {
            get { return this.propertyType; }
        }

        public override Type DeclaringType
        {
            get { return this.declaringType; }
        }

        public override string Name
        {
            get { return this.propertyName; }
        }

        public override Type ReflectedType
        {
            get { return this.DeclaringType; }
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            return null;
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            var fo = obj as FacetedViewModel;
            return fo[this.Name];
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            var fo = obj as FacetedViewModel;
            fo[this.Name] = value;
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return null;
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return null;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }
    }
}