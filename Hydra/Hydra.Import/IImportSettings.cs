namespace Hydra.Import
{
    public interface IImportSettings
    {
        ushort FloatingPointReadPrecision { get; }

        ushort FloatingPointWritePrecision { get; }
    }
}