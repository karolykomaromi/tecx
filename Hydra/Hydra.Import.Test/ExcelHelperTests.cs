namespace Hydra.Import.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Xunit;

    public class ExcelHelperTests
    {
        [Fact]
        public void Should_Find_SharedStringTable()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.Import001))
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
                {
                    SharedStringTable sharedStringTable = document.GetSharedStringTable();
                    Assert.NotNull(sharedStringTable);
                }
            }
        }

        [Fact]
        public void Should_Find_Metadata_Sheet()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.Import001))
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
                {
                    Worksheet meta = document.GetMetaDataWorksheet();
                    Assert.NotNull(meta);
                }
            }
        }

        [Fact]
        public void Should_Read_Cell_Value_With_Line_Breaks()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.ExcelHelper))
            {
                using (var document = SpreadsheetDocument.Open(stream, false))
                {
                    SharedStringTable sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                    Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>()
                        .First(s => string.Equals("Testzellen", s.Name, StringComparison.Ordinal));

                    var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                    Cell zelleMitZeilenUmbruch = worksheetPart.Worksheet.Descendants<Cell>()
                        .First(c => c.CellReference != null && string.Equals("A1", c.CellReference.Value, StringComparison.OrdinalIgnoreCase));

                    string actual = ExcelHelper.GetCellValue(zelleMitZeilenUmbruch, sharedStringTable);

                    string expected = "Erste Zeile,\r\n\r\nZweite Zeile";

                    Assert.Equal(expected, actual);
                }
            }
        }

        [Fact]
        public void Should_Read_Cell_Value_With_Protected_Line_Breaks()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.ExcelHelper))
            {
                using (var document = SpreadsheetDocument.Open(stream, false))
                {
                    SharedStringTable sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                    Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>()
                        .First(s => string.Equals("Testzellen", s.Name, StringComparison.Ordinal));

                    var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                    Cell zelleMitZeilenUmbruch = worksheetPart.Worksheet.Descendants<Cell>()
                        .First(c => c.CellReference != null && string.Equals("A2", c.CellReference.Value, StringComparison.OrdinalIgnoreCase));

                    string actual = ExcelHelper.GetCellValue(zelleMitZeilenUmbruch, sharedStringTable);

                    string expected = "Eins\r\n\r\nZwei";

                    Assert.Equal(expected, actual);
                }
            }
        }

        [Fact]
        public void Should_Get_Correct_Row_Index()
        {
            var c1 = new Cell { CellReference = "A1" };

            Assert.Equal(1u, ExcelHelper.GetRowIndex(c1));

            var c2 = new Cell { CellReference = "B129" };

            Assert.Equal(129u, ExcelHelper.GetRowIndex(c2));
        }

        [Fact]
        public void Should_Get_Correct_Column_Index()
        {
            var c1 = new Cell { CellReference = "A1" };

            Assert.Equal(1, ExcelHelper.GetColumnIndex(c1));

            var c2 = new Cell { CellReference = "B129" };

            Assert.Equal(2, ExcelHelper.GetColumnIndex(c2));

            var c3 = new Cell { CellReference = "BX1" };

            Assert.Equal(76, ExcelHelper.GetColumnIndex(c3));
        }
    }
}