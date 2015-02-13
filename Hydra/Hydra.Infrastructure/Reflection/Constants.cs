namespace Hydra.Infrastructure.Reflection
{
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

            public const MethodAttributes GetSetFromInterface = MethodAttributes.Public | 
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

            public const MethodAttributes Method = MethodAttributes.Public |
                                                   MethodAttributes.Final |
                                                   MethodAttributes.HideBySig |
                                                   MethodAttributes.NewSlot |
                                                   MethodAttributes.Virtual;
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
        }
    }
}