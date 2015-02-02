namespace Hydra.Import.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Xunit;
    using Xunit.Extensions;

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
                        .First(s => string.Equals("Cells", s.Name, StringComparison.Ordinal));

                    WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                    Cell zelleMitZeilenUmbruch = worksheetPart.Worksheet.Descendants<Cell>()
                        .First(c => c.CellReference != null && string.Equals("A1", c.CellReference.Value, StringComparison.OrdinalIgnoreCase));

                    string actual = ExcelHelper.GetCellValue(zelleMitZeilenUmbruch, sharedStringTable);

                    string expected = "First line,\r\n\r\nsecond line";

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
                        .First(s => string.Equals("Cells", s.Name, StringComparison.Ordinal));

                    var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                    Cell cellWithLineBreak = worksheetPart.Worksheet.Descendants<Cell>()
                        .First(c => c.CellReference != null && string.Equals("A2", c.CellReference.Value, StringComparison.OrdinalIgnoreCase));

                    string actual = ExcelHelper.GetCellValue(cellWithLineBreak, sharedStringTable);

                    string expected = "One\r\n\r\nTwo";

                    Assert.Equal(expected, actual);
                }
            }
        }

        [Fact]
        public void Should_Read_Cell_With_German_DateTime()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.ExcelHelper))
            {
                using (var document = SpreadsheetDocument.Open(stream, false))
                {
                    SharedStringTable sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                    Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>()
                        .First(s => string.Equals("Cells", s.Name, StringComparison.Ordinal));

                    var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                    Cell cellWithGermanDateTime = worksheetPart.Worksheet.Descendants<Cell>()
                        .First(c => c.CellReference != null && string.Equals("B1", c.CellReference, StringComparison.OrdinalIgnoreCase));

                    string actual = ExcelHelper.GetCellValue(cellWithGermanDateTime, sharedStringTable);

                    string expected = "2014-12-18T00:00:00.0000000";

                    Assert.Equal(expected, actual);
                }
            }
        }

        [Fact]
        public void Should_Read_Cell_With_US_DateTime()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.ExcelHelper))
            {
                using (var document = SpreadsheetDocument.Open(stream, false))
                {
                    SharedStringTable sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                    Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>()
                        .First(s => string.Equals("Cells", s.Name, StringComparison.Ordinal));

                    var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                    Cell cellWithGermanDateTime = worksheetPart.Worksheet.Descendants<Cell>()
                        .First(c => c.CellReference != null && string.Equals("B2", c.CellReference, StringComparison.OrdinalIgnoreCase));

                    string actual = ExcelHelper.GetCellValue(cellWithGermanDateTime, sharedStringTable);

                    string expected = "2014-12-18T00:00:00.0000000";

                    Assert.Equal(expected, actual);
                }
            }
        }

        [Theory]
        [InlineData("A1", 1u)]
        [InlineData("B129", 129u)]
        [InlineData("BX1", 1u)]
        public void Should_Get_Correct_Row_Index(string cellReference, uint rowIndex)
        {
            Assert.Equal(rowIndex, ExcelHelper.GetRowIndex(cellReference));
        }

        [Theory]
        [InlineData("A1", 1)]
        [InlineData("B129", 2)]
        [InlineData("BX1", 76)]
        public void Should_Get_Correct_Column_Index(string cellReference, int columnIndex)
        {
            Assert.Equal(columnIndex, ExcelHelper.GetColumnIndex(cellReference));
        }

        [Theory]
        [InlineData("A", 1)]
        [InlineData("B", 2)]
        [InlineData("BX", 76)]
        public void Should_Get_Correct_Column_Name(string columnName, int columnIndex)
        {
            Assert.Equal(columnName, ExcelHelper.GetColumnName(columnIndex));
        }
    }
}