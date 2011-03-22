using System;

namespace TecX.Unity.Configuration
{
    public static class GenericsHelper
    {
        public static bool CanBeCast(Type pluginType, Type pluggedType)
        {
            try
            {
                return CheckGenericType(pluggedType, pluginType);
            }
            catch (Exception e)
            {
                string message = string.Format(
                    "Could not Determine Whether Type '{0}' plugs into Type '{1}'",
                    pluginType.Name,
                    pluggedType.Name);

                throw new ApplicationException(message, e);
            }
        }

        private static bool CheckGenericType(Type pluggedType, Type pluginType)
        {
            if (pluginType.IsAssignableFrom(pluggedType))
            {
                return true;
            }

            // check interfaces
            foreach (Type type in pluggedType.GetInterfaces())
            {
                if (!type.IsGenericType)
                {
                    continue;
                }

                if (type.GetGenericTypeDefinition().Equals(pluginType))
                {
                    return true;
                }
            }

            if (pluggedType.BaseType.IsGenericType)
            {
                Type baseType = pluggedType.BaseType.GetGenericTypeDefinition();

                if (baseType.Equals(pluginType))
                {
                    return true;
                }
                else
                {
                    return CanBeCast(pluginType, baseType);
                }
            }

            return false;
        }
    }
}
