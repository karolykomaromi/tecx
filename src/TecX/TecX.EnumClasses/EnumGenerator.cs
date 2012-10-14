namespace TecX.EnumClasses
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;

    public class EnumGenerator
    {
        private const string EnumNamePostfix = "_Enum";
        private const string AssemblyName = "DynamicEnums";
        private const string DllExtension = ".dll";

        private static readonly ConstructorInfo DataContractAttributeCtor = typeof(DataContractAttribute).GetConstructor(Type.EmptyTypes);
        private static readonly ConstructorInfo EnumMemberAttributeCtor = typeof(EnumMemberAttribute).GetConstructor(Type.EmptyTypes);
        private static readonly PropertyInfo DataContractAttributeNameProperty = typeof(DataContractAttribute).GetProperty("Name");

        private readonly IDictionary<string, Type> enumTypes;
        private readonly AssemblyBuilder assembly;
        private readonly ModuleBuilder module;
        private readonly AssemblyName assemblyName;

        public EnumGenerator()
        {
            AppDomain domain = AppDomain.CurrentDomain;

            this.assemblyName = new AssemblyName(AssemblyName);

            this.assembly = domain.DefineDynamicAssembly(this.assemblyName, AssemblyBuilderAccess.RunAndSave);

            this.module = this.assembly.DefineDynamicModule(this.assemblyName.Name, this.assemblyName.Name + DllExtension);

            this.enumTypes = new ConcurrentDictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }

        public bool TryGetEnumByType(Type enumerationClassType, out Type enumType)
        {
            if (!typeof(Enumeration).IsAssignableFrom(enumerationClassType))
            {
                enumType = null;
                return false;
            }

            string enumName = enumerationClassType.Name + EnumNamePostfix;

            if (this.enumTypes.TryGetValue(enumName, out enumType))
            {
                return true;
            }

            FieldInfo[] fields = enumerationClassType
                .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)
                .Where(f => typeof(Enumeration).IsAssignableFrom(f.FieldType))
                .ToArray();

            EnumBuilder @enum = this.module.DefineEnum(enumName, TypeAttributes.Public, typeof(int));

            var attribute = new CustomAttributeBuilder(DataContractAttributeCtor, new object[0], new[] { DataContractAttributeNameProperty }, new object[] { enumerationClassType.Name });

            @enum.SetCustomAttribute(attribute);

            foreach (var field in fields)
            {
                FieldBuilder builder = @enum.DefineLiteral(field.Name, ((Enumeration)field.GetValue(null)).Value);

                builder.SetCustomAttribute(EnumMemberAttributeCtor, new byte[0]);
            }

            enumType = @enum.CreateType();

            this.enumTypes.Add(enumName, enumType);

            this.assembly.Save(this.assemblyName.Name + DllExtension);

            return true;
        }

        public bool IsGeneratedEnum(Type type)
        {
            return type.IsEnum && type.Name.EndsWith(EnumNamePostfix);
        }
    }
}