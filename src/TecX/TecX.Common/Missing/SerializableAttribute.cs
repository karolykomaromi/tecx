using System;

namespace System
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
    public class SerializableAttribute : Attribute
    {
    }
}
