namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public class ResxFilesResourceManager : IResourceManager
    {
        private readonly Type resxType;

        public ResxFilesResourceManager(Type resxType)
        {
            Contract.Requires(resxType != null);

            this.resxType = resxType;
        }

        public string this[string key]
        {
            get
            {
                PropertyInfo property = this.resxType.GetProperty(key, BindingFlags.Static | BindingFlags.Public);

                if (property != null)
                {
                    MethodInfo getter = property.GetGetMethod();

                    if (getter != null)
                    {
                        string value = getter.Invoke(null, null) as string;

                        if (value != null)
                        {
                            return value;
                        }
                    }
                }

                return key;
            }
        }
    }
}