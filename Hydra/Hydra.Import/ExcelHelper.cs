﻿namespace Hydra.Import
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text.RegularExpressions;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    public static class ExcelHelper
    {
        private static readonly Regex NewLine = new Regex("(?<!\r)\n", RegexOptions.Compiled);

        private static readonly Regex LettersOnly = new Regex("[a-zA-Z]+", RegexOptions.Compiled);

        private static readonly Regex NumbersOnly = new Regex(@"\d+", RegexOptions.Compiled);

        public static string GetCellValue(Cell cell, SharedStringTable sharedStringTable)
        {
            Contract.Requires(cell != null);
            Contract.Requires(sharedStringTable != null);

            string value = cell.InnerText ?? string.Empty;

            int sharedStringIndex;
            if (cell.DataType != null &&
                cell.DataType.Value == CellValues.SharedString &&
                int.TryParse(value, out sharedStringIndex))
            {
                value = sharedStringTable.ElementAt(sharedStringIndex).InnerText ?? string.Empty;
            }

            if (value.StartsWith("'", StringComparison.Ordinal))
            {
                value = value.Substring(1);
            }

            // http://stackoverflow.com/questions/31053/regex-c-replace-n-with-r-n
            value = NewLine.Replace(value, "\r\n");

            value = value.Trim();

            return value;
        }

        public static string GetColumnName(Cell cell)
        {
            Contract.Requires(cell != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(cell.CellReference));

            return ExcelHelper.GetColumnName(cell.CellReference.Value);
        }

        public static string GetColumnName(string cellReference)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(cellReference));

            Match match = ExcelHelper.LettersOnly.Match(cellReference);

            return match.Value;
        }

        public static uint GetRowIndex(Cell cell)
        {
            Contract.Requires(cell != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(cell.CellReference));

            return ExcelHelper.GetRowIndex(cell.CellReference);
        }

        public static uint GetRowIndex(string cellReference)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(cellReference));

            Match match = ExcelHelper.NumbersOnly.Match(cellReference);

            uint rowIndex = uint.Parse(match.Value);

            return rowIndex;
        }

        public static int GetColumnIndex(Cell cell)
        {
            Contract.Requires(cell != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(cell.CellReference));

            return ExcelHelper.GetColumnIndex(cell.CellReference.Value);
        }

        public static int GetColumnIndex(string cellReference)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(cellReference));

            string columnName = ExcelHelper.GetColumnName(cellReference);

            columnName = columnName.ToUpperInvariant();

            int columnIndex = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                columnIndex *= 26;
                columnIndex += columnName[i] - 'A' + 1;
            }

            return columnIndex;
        }

        public static SharedStringTable GetSharedStringTable(this SpreadsheetDocument document)
        {
            Contract.Requires(document != null);

            return document.WorkbookPart.GetPartsOfType<SharedStringTablePart>()
                           .Where(s => s.SharedStringTable != null)
                           .Select(s => s.SharedStringTable)
                           .FirstOrDefault();
        }

        public static Worksheet GetMetaDataWorksheet(this SpreadsheetDocument document)
        {
            Contract.Requires(document != null);
            Contract.Requires(document.WorkbookPart != null);
            Contract.Requires(document.WorkbookPart.Workbook != null);

            Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>()
                .FirstOrDefault(s => string.Equals("Meta", s.Name, StringComparison.OrdinalIgnoreCase));

            if (sheet == null)
            {
                return null;
            }

            WorksheetPart wsp = document.WorkbookPart.GetPartById(sheet.Id) as WorksheetPart;

            if (wsp == null)
            {
                return null;
            }

            Worksheet ws = wsp.Worksheet;

            return ws;
        }
    }
}