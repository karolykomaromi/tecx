namespace Hydra.Import.Settings
{
    public class ExcelImportSettings : ImportSettings, IExcelImportSettings
    {
        public ExcelImportSettings()
        {
            this.PropertyNamesRowIndex = 1;
        }

        public uint PropertyNamesRowIndex { get; set; }
    }
}