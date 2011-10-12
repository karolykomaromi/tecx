using System;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.Configuration.Common
{
    public static class Constructor
    {
        public static bool HasConstructors(Type type)
        {
            Guard.AssertNotNull(type, "type");

            return GetGreediestConstructor(type) != null;
        }

        public static ConstructorInfo GetGreediestConstructor(Type type)
        {
            Guard.AssertNotNull(type, "type");

            ConstructorInfo returnValue = null;

            foreach (ConstructorInfo constructor in type.GetConstructors())
            {
                if (returnValue == null)
                {
                    returnValue = constructor;
                }
                else if (constructor.GetParameters().Length > returnValue.GetParameters().Length)
                {
                    returnValue = constructor;
                }
            }

            return returnValue;
        }
    }
}
