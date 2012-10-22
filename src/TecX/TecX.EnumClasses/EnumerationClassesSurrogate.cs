namespace TecX.EnumClasses
{
    using System;
    using System.CodeDom;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    public class EnumerationClassesSurrogate : IDataContractSurrogate
    {
        private readonly EnumGenerator generator;

        public EnumerationClassesSurrogate()
        {
            this.generator = new EnumGenerator();
        }

        public Type GetDataContractType(Type type)
        {
            if (typeof(Enumeration).IsAssignableFrom(type))
            {
                Type enumType;
                if (this.generator.TryGetEnumByType(type, out enumType))
                {
                    return enumType;
                }
            }

            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            Enumeration enumeration = obj as Enumeration;
            if (enumeration != null)
            {
                Type enumType;
                if (this.generator.TryGetEnumByType(obj.GetType(), out enumType) && Enum.IsDefined(enumType, enumeration.Name))
                {
                    return Enum.Parse(enumType, enumeration.Name);
                }
            }

            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            Type objType = obj.GetType();

            if (this.generator.IsGeneratedEnum(objType) && typeof(Enumeration).IsAssignableFrom(targetType))
            {
                string name = Enum.GetName(objType, obj);

                return Enumeration.FromName(targetType, name);
            }

            return obj;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }
    }
}