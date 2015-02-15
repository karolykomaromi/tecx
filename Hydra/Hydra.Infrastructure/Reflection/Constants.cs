namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Reflection;

    public static class Constants
    {
        public static class Attributes
        {
            /// <summary>
            /// The property "set" and property "get" methods require a special
            /// set of attributes.
            /// </summary>
            public const MethodAttributes GetSet = MethodAttributes.Public |
                                                   MethodAttributes.SpecialName |
                                                   MethodAttributes.HideBySig;

            public const MethodAttributes GetSetFromInterface = MethodAttributes.Private | 
                                                                MethodAttributes.Final | 
                                                                MethodAttributes.HideBySig | 
                                                                MethodAttributes.SpecialName | 
                                                                MethodAttributes.NewSlot | 
                                                                MethodAttributes.Virtual;

            public const TypeAttributes GeneratedType = TypeAttributes.Public |
                                                        TypeAttributes.Class |
                                                        TypeAttributes.AutoClass |
                                                        TypeAttributes.AnsiClass |
                                                        TypeAttributes.BeforeFieldInit |
                                                        TypeAttributes.AutoLayout;

            public const MethodAttributes ExplicitMethod = MethodAttributes.Private |
                                                           MethodAttributes.Final |
                                                           MethodAttributes.HideBySig |
                                                           MethodAttributes.NewSlot |
                                                           MethodAttributes.Virtual;

            public const MethodAttributes Ctor = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;

            public const FieldAttributes ReadonlyField = FieldAttributes.Private | FieldAttributes.InitOnly;
        }

        public static class Constructors
        {
            public static readonly ConstructorInfo NotImplementedException = typeof(NotImplementedException).GetConstructor(new Type[0]);

            public static readonly ConstructorInfo Object = typeof(object).GetConstructor(new Type[0]);
        }

        public static class Names
        {
            /// <summary>
            /// GeneratedProxies
            /// </summary>
            public const string AssemblyName = "GeneratedProxies";

            /// <summary>
            /// GeneratedProxies.dll
            /// </summary>
            public const string AssemblyFileName = AssemblyName + ".dll";

            public const string SetterPrefix = "set_";

            public const string GetterPrefix = "get_";

            public const string TargetField = "target";
        }
    }
}