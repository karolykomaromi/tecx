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

        public string this[ResxKey key]
        {
            get
            {
                string rsk = key.ToString();

                int lastIndexOfDot = Math.Max(0, rsk.LastIndexOf('.'));

                if (!this.resxType.FullName.StartsWith(rsk.Substring(0, lastIndexOfDot), StringComparison.OrdinalIgnoreCase))
                {
                    return rsk;
                }

                PropertyInfo property = this.resxType.GetProperty(rsk.Substring(lastIndexOfDot + 1), BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (property == null)
                {
                    return rsk;
                }

                MethodInfo getter = property.GetGetMethod();

                if (getter == null)
                {
                    return rsk;
                }

                string value = getter.Invoke(null, null) as string;

                if (value != null)
                {
                    return value;
                }

                return rsk;
            }
        }
    }
}