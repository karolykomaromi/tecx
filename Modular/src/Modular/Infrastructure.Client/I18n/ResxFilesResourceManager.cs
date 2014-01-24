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
                string rxk = key.ToString();

                int lastIndexOfDot = Math.Max(0, rxk.LastIndexOf('.'));

                if (!this.resxType.FullName.StartsWith(rxk.Substring(0, lastIndexOfDot), StringComparison.OrdinalIgnoreCase))
                {
                    return rxk;
                }

                PropertyInfo property = this.resxType.GetProperty(rxk.Substring(lastIndexOfDot + 1), BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (property == null)
                {
                    return rxk;
                }

                MethodInfo getter = property.GetGetMethod();

                if (getter == null)
                {
                    return rxk;
                }

                string value = getter.Invoke(null, null) as string;

                if (value != null)
                {
                    return value;
                }

                return rxk;
            }
        }
    }
}