namespace Hydra.Import
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using DocumentFormat.OpenXml.Spreadsheet;

    public class DateTimeCellValueWriter : IExcelCellValueWriter
    {
        private readonly PropertyInfo property;

        public DateTimeCellValueWriter(PropertyInfo property)
        {
            Contract.Requires(property != null);

            this.property = property;
        }

        public string PropertyName
        {
            get { return this.Property.Name; }
        }

        protected PropertyInfo Property
        {
            get { return this.property; }
        }

        public ImportMessage Write(Cell target, object value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            if (value == null)
            {
                return ImportMessage.Empty;
            }

            if (!(value is DateTime))
            {
                string message = string.Format(
                    Properties.Resources.ErrorValueIsOfWrongType, 
                    typeof(DateTime).AssemblyQualifiedName, 
                    value.GetType().AssemblyQualifiedName);

                return new Error(message);
            }

            DateTime dt = (DateTime)value;

            //target.DataType = CellValues.Date;

            //string dateString = dt.ToOADate().ToString("R", targetCulture);
            string dateString = dt.ToOADate().ToString(CultureInfo.InvariantCulture);

            CellValue cellValue = new CellValue(dateString);

            target.AppendChild(cellValue);

            return ImportMessage.Empty;
        }
    }
}