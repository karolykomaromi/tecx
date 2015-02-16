namespace Hydra.Import.Settings
{
    using System.Globalization;

    public class ImportSettings : IImportSettings
    {
        public ImportSettings()
        {
            this.TargetCulture = CultureInfo.CurrentCulture;

            this.SourceCulture = CultureInfo.CurrentCulture;
        }

        public virtual ushort FloatingPointReadPrecision { get; set; }

        public virtual ushort FloatingPointWritePrecision { get; set; }

        public virtual CultureInfo TargetCulture { get; set; }

        public virtual CultureInfo SourceCulture { get; set; }
    }
}