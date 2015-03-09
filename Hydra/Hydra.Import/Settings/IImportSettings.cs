namespace Hydra.Import.Settings
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [ContractClass(typeof(ImportSettingsContract))]
    public interface IImportSettings
    {
        ushort FloatingPointReadPrecision { get; }

        ushort FloatingPointWritePrecision { get; }

        CultureInfo TargetCulture { get; }

        CultureInfo SourceCulture { get; }
    }

    [ContractClassFor(typeof(IImportSettings))]
    internal abstract class ImportSettingsContract : IImportSettings
    {
        public abstract ushort FloatingPointReadPrecision { get; }

        public abstract ushort FloatingPointWritePrecision { get; }

        public CultureInfo TargetCulture
        {
            get
            {
                Contract.Ensures(Contract.Result<CultureInfo>() != null);

                return CultureInfo.InvariantCulture;
            }
        }

        public CultureInfo SourceCulture
        {
            get
            {
                Contract.Ensures(Contract.Result<CultureInfo>() != null);

                return CultureInfo.InvariantCulture;
            }
        }
    }
}