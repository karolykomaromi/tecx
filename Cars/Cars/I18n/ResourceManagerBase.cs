namespace Cars.I18n
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;

    public abstract class ResourceManagerBase : IResourceManager
    {
        private readonly string baseName;

        protected ResourceManagerBase(string baseName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(baseName));

            this.baseName = baseName;
        }

        public string BaseName
        {
            get { return this.baseName; }
        }

        public static IResourceManager Null(string baseName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(baseName));
            Contract.Ensures(Contract.Result<IResourceManager>() != null);

            return new NullResourceManager(baseName);
        }

        public abstract string GetString(string name, CultureInfo culture);

        public abstract object GetObject(string name, CultureInfo culture);

        protected virtual string GetKey(string name, CultureInfo culture)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Requires(culture != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return (this.baseName + "." + name + "_" + culture.Name).ToUpperInvariant();
        }

        [DebuggerDisplay("BaseName={BaseName}")]
        private class NullResourceManager : ResourceManagerBase
        {
            public NullResourceManager(string baseName)
                : base(baseName)
            {
            }

            public override string GetString(string name, CultureInfo culture)
            {
                return this.GetKey(name, culture);
            }

            public override object GetObject(string name, CultureInfo culture)
            {
                return Missing.Value;
            }
        }
    }
}