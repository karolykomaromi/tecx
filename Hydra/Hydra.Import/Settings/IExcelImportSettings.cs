namespace Hydra.Import.Settings
{
    public interface IExcelImportSettings : IImportSettings
    {
        uint PropertyNamesRowIndex { get; }
    }
}