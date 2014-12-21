namespace Hydra.Import
{
    public interface IExcelImportSettings : IImportSettings
    {
        uint PropertyNamesRowIndex { get; }
    }
}