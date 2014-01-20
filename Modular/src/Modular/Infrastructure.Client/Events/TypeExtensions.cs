namespace Infrastructure.Events
{
    using System;

    internal static class TypeExtensions
    {
        public static Type[] FindInterfaces(this Type type, TypeFilter filter, object filterCriteria)
        {
            Type[] interfaces = type.GetInterfaces();
            int num = 0;
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (!filter(interfaces[i], filterCriteria))
                {
                    interfaces[i] = null;
                }
                else
                {
                    num++;
                }
            }

            if (num == interfaces.Length)
            {
                return interfaces;
            }

            Type[] typeArray2 = new Type[num];
            num = 0;
            for (int j = 0; j < interfaces.Length; j++)
            {
                if (interfaces[j] != null)
                {
                    typeArray2[num++] = interfaces[j];
                }
            }

            return typeArray2;
        }
    }
}