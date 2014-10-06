namespace Hydra.Infrastructure.I18n
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [ContractClass(typeof(ResourceManagerContract))]
    public interface IResourceManager
    {
        string BaseName { get; }

        string GetString(string name, CultureInfo culture);
    }

    [ContractClassFor(typeof(IResourceManager))]
    internal abstract class ResourceManagerContract : IResourceManager
    {
        public string BaseName
        {
            get { return string.Empty; }
        }

        public string GetString(string name, CultureInfo culture)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Requires(culture != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return string.Empty;
        }
    }
}
