namespace Infrastructure.I18n
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [ContractClass(typeof(ResourceManagerContract))]
    public interface IResourceManager
    {
        string GetString(string name, CultureInfo culture);
    }

    [ContractClassFor(typeof(IResourceManager))]
    internal abstract class ResourceManagerContract : IResourceManager
    {
        public string GetString(string name, CultureInfo culture)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(culture != null);

            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return " ";
        }
    }
}
