namespace Infrastructure.Triggers
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using Infrastructure.Converters;

    public class FormatDateTimeColumn : TriggerAction<DataGrid>
    {
        protected override void Invoke(object parameter)
        {
            DataGridAutoGeneratingColumnEventArgs args = parameter as DataGridAutoGeneratingColumnEventArgs;

            if (args == null)
            {
                return;
            }

            DataGridTextColumn column;
            if (args.PropertyType == typeof(DateTime) && (column = args.Column as DataGridTextColumn) != null)
            {
                if (column.Binding.Converter as DateTimeFormatConverter == null)
                {
                    column.Binding.Converter = new DateTimeFormatConverter();
                }
            }
        }
    }
}