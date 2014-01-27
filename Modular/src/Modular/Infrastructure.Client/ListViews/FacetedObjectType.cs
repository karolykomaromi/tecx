namespace Infrastructure.ListViews
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A custom System.Type implementation that can provide different Properties at runtime.  All operations except those related to
    /// Property logic delegated to the type of TSource
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class FacetedObjectType<TSource> : Type
    {
        private readonly IDictionary<string, Facet> dynamicFacets;

        private IDictionary<string, DynamicPropertyInfo> dynamicPropertyInfos;

        public FacetedObjectType(List<Facet> dynamicFacets)
        {
            this.ProxyTargetType = typeof(TSource);
            this.dynamicFacets = new Dictionary<string, Facet>();
            dynamicFacets.ForEach(f =>
            {
                this.dynamicFacets[f.PropertyName] = f;
            });
        }

        public Type ProxyTargetType { get; private set; }

        public override Assembly Assembly
        {
            get { return this.ProxyTargetType.Assembly; }
        }

        public override string AssemblyQualifiedName
        {
            get { return this.ProxyTargetType.AssemblyQualifiedName; }
        }

        public override Type BaseType
        {
            get { return this.ProxyTargetType.BaseType; }
        }

        public override string FullName
        {
            get { return this.ProxyTargetType.FullName; }
        }

        public override Guid GUID
        {
            get { return this.ProxyTargetType.GUID; }
        }

        public override Module Module
        {
            get { return this.ProxyTargetType.Module; }
        }

        public override string Namespace
        {
            get { return this.ProxyTargetType.Namespace; }
        }

        public override Type UnderlyingSystemType
        {
            get { return this.ProxyTargetType.UnderlyingSystemType; }
        }

        public override string Name
        {
            get { return this.ProxyTargetType.Name; }
        }

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            return this.ProxyTargetType.GetConstructors();
        }

        public override Type GetElementType()
        {
            return this.ProxyTargetType.GetElementType();
        }

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
        {
            return this.ProxyTargetType.GetEvent(name, bindingAttr);
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            return this.ProxyTargetType.GetEvents(bindingAttr);
        }

        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return this.ProxyTargetType.GetField(name, bindingAttr);
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return this.ProxyTargetType.GetFields(bindingAttr);
        }

        public override Type GetInterface(string name, bool ignoreCase)
        {
            return this.ProxyTargetType.GetInterface(name, ignoreCase);
        }

        public override Type[] GetInterfaces()
        {
            return this.ProxyTargetType.GetInterfaces();
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            var members = this.ProxyTargetType.GetMembers(bindingAttr);
            if (BindingFlags.Instance == (bindingAttr & BindingFlags.Instance) && BindingFlags.Public == (bindingAttr & BindingFlags.Public))
            {
                var dynamicMembers = this.GetPublicDynamicProperties();
                var allMembers = new List<MemberInfo>();
                allMembers.AddRange(members);
                allMembers.AddRange(dynamicMembers);

                return allMembers.ToArray();
            }

            return members;
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return this.ProxyTargetType.GetMethods(bindingAttr);
        }

        public override Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            return this.ProxyTargetType.GetNestedType(name, bindingAttr);
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return this.ProxyTargetType.GetNestedTypes(bindingAttr);
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            var properties = this.ProxyTargetType.GetProperties(bindingAttr).Where(this.ShouldShowInDynamicListView).ToArray();

            if (BindingFlags.Instance == (bindingAttr & BindingFlags.Instance) && BindingFlags.Public == (bindingAttr & BindingFlags.Public))
            {
                var dynamicProperties = this.GetPublicDynamicProperties();
                var allprops = new List<PropertyInfo>();
                allprops.AddRange(properties);
                allprops.AddRange(dynamicProperties);

                return allprops.ToArray();
            }

            return properties;
        }

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            return this.ProxyTargetType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return this.ProxyTargetType.GetCustomAttributes(attributeType, inherit);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return this.ProxyTargetType.GetCustomAttributes(inherit);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return this.ProxyTargetType.IsDefined(attributeType, inherit);
        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            return this.ProxyTargetType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            return TypeAttributes.Class | TypeAttributes.Public;
        }

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            return this.ProxyTargetType.GetConstructor(bindingAttr, binder, types, modifiers);
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            if (!this.dynamicFacets.ContainsKey(name))
            {
                if (null == types)
                {
                    return this.ProxyTargetType.GetProperty(name, bindingAttr);
                }

                return this.ProxyTargetType.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
            }

            return this.CreateFromFacet(this.dynamicFacets[name]);
        }

        protected override bool HasElementTypeImpl()
        {
            return this.ProxyTargetType.HasElementType;
        }

        protected override bool IsArrayImpl()
        {
            return this.ProxyTargetType.IsArray;
        }

        protected override bool IsByRefImpl()
        {
            return this.ProxyTargetType.IsByRef;
        }

        protected override bool IsCOMObjectImpl()
        {
            return this.ProxyTargetType.IsCOMObject;
        }

        protected override bool IsPointerImpl()
        {
            return this.ProxyTargetType.IsPointer;
        }

        protected override bool IsPrimitiveImpl()
        {
            return this.ProxyTargetType.IsPrimitive;
        }

        protected List<string> GetPublicDynamicPropertyNames()
        {
            var dynamicPropNames = this.dynamicFacets.Keys.ToList();
            return dynamicPropNames;
        }

        protected List<PropertyInfo> GetPublicDynamicProperties()
        {
            this.EnsurePropertyInfo();
            return this.dynamicPropertyInfos.Values.Cast<PropertyInfo>().ToList();
        }

        protected DynamicPropertyInfo CreateFromFacet(Facet facet)
        {
            return new DynamicPropertyInfo(facet.PropertyType, typeof(FacetedViewModel), facet.PropertyName);
        }

        private void EnsurePropertyInfo()
        {
            if (null == this.dynamicPropertyInfos)
            {
                this.dynamicPropertyInfos = new Dictionary<string, DynamicPropertyInfo>();

                foreach (var facet in this.dynamicFacets.Values)
                {
                    var dynamicPropInfo = this.CreateFromFacet(facet);
                    this.dynamicPropertyInfos[facet.PropertyName] = dynamicPropInfo;
                }
            }
        }

        private bool ShouldShowInDynamicListView(PropertyInfo property)
        {
            if (string.Equals(property.Name, "CommandManager", StringComparison.Ordinal))
            {
                return false;
            }

            if (string.Equals(property.Name, "Item", StringComparison.Ordinal))
            {
                return false;
            }

            if (string.Equals(property.Name, "ResourceManager", StringComparison.Ordinal))
            {
                return false;
            }

            return true;
        }
    }
}