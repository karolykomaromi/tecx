namespace Infrastructure.Triggers
{
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using Infrastructure.ViewModels;

    public class TranslateColumnHeader : TriggerAction<DataGrid>
    {
        protected override void Invoke(object parameter)
        {
            DataGridAutoGeneratingColumnEventArgs args = parameter as DataGridAutoGeneratingColumnEventArgs;

            if (args == null)
            {
                return;
            }

            DynamicListViewModel vm = this.AssociatedObject.DataContext as DynamicListViewModel;

            if (vm == null)
            {
                return;
            }

            // search for facet identifying this property, use resourcemanager to load resx and put it in the header property of the column.
            string header = vm.TranslatePropertyName(args.PropertyName);

            args.Column.Header = header;
        }
    }
}
