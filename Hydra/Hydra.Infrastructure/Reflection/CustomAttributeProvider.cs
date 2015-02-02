namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Reflection;

    public class CustomAttributeProvider
    {
        public static readonly ICustomAttributeProvider Null = new NullCustomAttributeProvider();

        private class NullCustomAttributeProvider : ICustomAttributeProvider
        {
            public object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return new object[0];
            }

            public object[] GetCustomAttributes(bool inherit)
            {
                return new object[0];
            }

            public bool IsDefined(Type attributeType, bool inherit)
            {
                return false;
            }
        }
    }
}