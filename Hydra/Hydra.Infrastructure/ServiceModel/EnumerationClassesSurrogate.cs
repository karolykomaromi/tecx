namespace Hydra.Infrastructure.ServiceModel
{
    using System;
    using System.CodeDom;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Hydra.Infrastructure;

    public class EnumerationClassesSurrogate : IDataContractSurrogate
    {
        private readonly EnumGenerator generator;

        public EnumerationClassesSurrogate()
        {
            this.generator = new EnumGenerator();
        }

        public EnumGenerator Generator
        {
            get { return this.generator; }
        }

        Type IDataContractSurrogate.GetDataContractType(Type type)
        {
            if (typeof(IEnumeration).IsAssignableFrom(type))
            {
                Type enumType;
                if (this.Generator.TryGetEnumByType(type, out enumType))
                {
                    return enumType;
                }
            }

            return type;
        }

        object IDataContractSurrogate.GetObjectToSerialize(object obj, Type targetType)
        {
            IEnumeration enumeration = obj as IEnumeration;
            if (enumeration != null)
            {
                Type enumType;
                if (this.Generator.TryGetEnumByType(obj.GetType(), out enumType) && Enum.IsDefined(enumType, enumeration.Name))
                {
                    return Enum.Parse(enumType, enumeration.Name);
                }
            }

            return obj;
        }

        object IDataContractSurrogate.GetDeserializedObject(object obj, Type targetType)
        {
            Type objType = obj.GetType();

            if (this.Generator.IsGeneratedEnum(objType) && typeof(IEnumeration).IsAssignableFrom(targetType))
            {
                string name = Enum.GetName(objType, obj);

                object o;
                if (Enumeration.TryParse(targetType, name, out o))
                {
                    return o;
                }
            }

            return obj;
        }

        object IDataContractSurrogate.GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        object IDataContractSurrogate.GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        void IDataContractSurrogate.GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
        }

        Type IDataContractSurrogate.GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        CodeTypeDeclaration IDataContractSurrogate.ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }
    }
}