namespace Hydra.Import.Settings
{
    using System.Globalization;

    public interface IImportSettings
    {
        ushort FloatingPointReadPrecision { get; }

        ushort FloatingPointWritePrecision { get; }

        CultureInfo TargetCulture { get; }

        CultureInfo SourceCulture { get; }
    }
}