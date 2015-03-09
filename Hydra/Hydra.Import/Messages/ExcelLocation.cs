namespace Hydra.Import.Messages
{
    using System.Diagnostics.Contracts;

    public class ExcelLocation : Location
    {
        private readonly string sheetName;
        private readonly string columnName;
        private readonly uint rowIndex;

        public ExcelLocation(string sheetName, string cellReference)
            : this(sheetName, ExcelHelper.GetColumnName(cellReference), ExcelHelper.GetRowIndex(cellReference))
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(sheetName));
        }

        public ExcelLocation(string sheetName, string columnName, uint rowIndex)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(sheetName));
            Contract.Requires(!string.IsNullOrWhiteSpace(columnName));
            Contract.Requires(rowIndex > 0);

            this.sheetName = sheetName;
            this.columnName = columnName;
            this.rowIndex = rowIndex;
        }

        public string SheetName
        {
            get { return this.sheetName; }
        }

        public string ColumnName
        {
            get { return this.columnName; }
        }

        public uint RowIndex
        {
            get { return this.rowIndex; }
        }

        public override string ToString()
        {
            string s = 
                Properties.Resources.SheetName + 
                " " + 
                this.SheetName + 
                "; " + 
                Properties.Resources.ColumnName +
                " " 
                + this.ColumnName + 
                "; " + 
                Properties.Resources.RowIndex + 
                " " + 
                this.RowIndex;

            return s;
        }
    }
}