namespace Hydra.Import
{
    public class ImportSettings : IImportSettings
    {
        public virtual ushort FloatingPointReadPrecision { get; set; }

        public virtual ushort FloatingPointWritePrecision { get; set; }
    }
}