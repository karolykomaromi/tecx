namespace Hydra.Import
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Hydra.Infrastructure;

    public class StyleSheetBuilder : Builder<Stylesheet>
    {
        private CultureInfo culture;

        public StyleSheetBuilder()
        {
            this.culture = CultureInfo.CurrentCulture;
        }

        public StyleSheetBuilder WithCulture(CultureInfo culture)
        {
            Contract.Requires(culture != null);
            Contract.Ensures(Contract.Result<StyleSheetBuilder>() != null);

            this.culture = culture;

            return this;
        }

        public override Stylesheet Build()
        {
            Stylesheet stylesheet = new Stylesheet();

            // Numbering formats (x:numFmts)
            stylesheet.InsertAt<NumberingFormats>(new NumberingFormats(), 0);

            // Currency
            stylesheet.GetFirstChild<NumberingFormats>().InsertAt<NumberingFormat>(
                new NumberingFormat
                {
                    NumberFormatId = 164,
                    FormatCode = "#" + this.culture.NumberFormat.CurrencyGroupSeparator + "##0 " + this.culture.NumberFormat.CurrencyDecimalSeparator + "00" + "\\ \"" + this.culture.NumberFormat.CurrencySymbol + "\""
                },
                0);

            // Fonts (x:fonts)
            stylesheet.InsertAt<Fonts>(new Fonts(), 1);
            stylesheet.GetFirstChild<Fonts>().InsertAt<Font>(
                new Font
                {
                    FontSize = new FontSize { Val = 11 },
                    FontName = new FontName { Val = "Calibri" }
                },
                0);

            // Fills (x:fills)
            stylesheet.InsertAt<Fills>(new Fills(), 2);
            stylesheet.GetFirstChild<Fills>().InsertAt<Fill>(
                new Fill
                {
                    PatternFill = new PatternFill
                    {
                        PatternType = new EnumValue<PatternValues> { Value = PatternValues.None }
                    }
                },
                0);

            // Borders (x:borders)
            stylesheet.InsertAt<Borders>(new Borders(), 3);
            stylesheet.GetFirstChild<Borders>().InsertAt<Border>(
                new Border
                {
                    LeftBorder = new LeftBorder(),
                    RightBorder = new RightBorder(),
                    TopBorder = new TopBorder(),
                    BottomBorder = new BottomBorder(),
                    DiagonalBorder = new DiagonalBorder()
                },
                0);

            // Cell style formats (x:CellStyleXfs)
            stylesheet.InsertAt<CellStyleFormats>(new CellStyleFormats(), 4);

            stylesheet.GetFirstChild<CellStyleFormats>().InsertAt<CellFormat>(
                new CellFormat
                {
                    NumberFormatId = 0,
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0
                },
                0);

            // Cell formats (x:CellXfs)
            stylesheet.InsertAt<CellFormats>(new CellFormats(), 5);

            // General text
            stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
                new CellFormat
                {
                    FormatId = 0,
                    NumberFormatId = 0
                },
                CellFormatIndices.Text);

            // Date
            stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
                new CellFormat
                {
                    ApplyNumberFormat = true,
                    FormatId = 0,
                    NumberFormatId = 22,
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0
                },
                CellFormatIndices.Date);

            // Currency
            stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
                new CellFormat
                {
                    ApplyNumberFormat = true,
                    FormatId = 0,
                    NumberFormatId = 164,
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0
                },
                CellFormatIndices.Currency);

            // Percentage
            stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
                new CellFormat()
                {
                    ApplyNumberFormat = true,
                    FormatId = 0,
                    NumberFormatId = 10,
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0
                },
                CellFormatIndices.Percentage);

            return stylesheet;
        }
    }
}