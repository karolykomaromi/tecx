namespace Hydra.Infrastructure.Configuration
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(ISettingsProvider))]
    internal abstract class SettingsProviderContract : ISettingsProvider
    {
        public SettingsCollection GetSettings()
        {
            Contract.Ensures(Contract.Result<SettingsCollection>() != null);

            return new SettingsCollection();
        }
    }

    [ContractClass(typeof(SettingsProviderContract))]
    public interface ISettingsProvider
    {
        SettingsCollection GetSettings();
    }
}