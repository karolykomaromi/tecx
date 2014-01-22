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
                int lastIndexOfDot = Math.Max(0, key.LastIndexOf('.'));

                if (!this.resxType.FullName.StartsWith(key.Substring(0, lastIndexOfDot), StringComparison.OrdinalIgnoreCase))
                {
                    return key;
                }

                PropertyInfo property = this.resxType.GetProperty(key.Substring(lastIndexOfDot + 1), BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (property == null)
                {
                    return key;
                }

                MethodInfo getter = property.GetGetMethod();

                if (getter == null)
                {
                    return key;
                }

                string value = getter.Invoke(null, null) as string;

                if (value != null)
                {
                    return value;
                }

                return key;
            }
        }
    }
}