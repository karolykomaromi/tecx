namespace TecX.Unity.Proxies
{
    using System.Reflection;

    public static class Constants
    {
        /// <summary>
        /// The property "set" and property "get" methods require a special
        /// set of attributes.
        /// </summary>
        public const MethodAttributes GetSetAttributes = MethodAttributes.Public |
                                                         MethodAttributes.SpecialName |
                                                         MethodAttributes.HideBySig;

        public const TypeAttributes TypeAttr = TypeAttributes.Public |
                                               TypeAttributes.Class |
                                               TypeAttributes.AutoClass |
                                               TypeAttributes.AnsiClass |
                                               TypeAttributes.BeforeFieldInit |
                                               TypeAttributes.AutoLayout;

        public const MethodAttributes MethodAttr = MethodAttributes.Public |
                                                   MethodAttributes.HideBySig |
                                                   MethodAttributes.NewSlot |
                                                   MethodAttributes.Virtual |
                                                   MethodAttributes.Final;
    }
}
