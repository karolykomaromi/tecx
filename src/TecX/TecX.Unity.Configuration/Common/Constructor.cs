using System;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.Configuration.Common
{
    public static class Constructor
    {
        public static bool HasConstructors(Type pluggedType)
        {
            Guard.AssertNotNull(pluggedType, "pluggedType");

            return GetGreediestConstructor(pluggedType) != null;
        }

        private static ConstructorInfo GetGreediestConstructor(Type pluggedType)
        {
            ConstructorInfo returnValue = null;

            foreach (ConstructorInfo constructor in pluggedType.GetConstructors())
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
