namespace Infrastructure.Converters
{
    using System.Windows.Controls;
    using GalaSoft.MvvmLight.Command;
    using Infrastructure.Commands;
    using Infrastructure.ViewModels;

    public class DynamicListViewRowLoadingConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            DataGridRowEventArgs args = (DataGridRowEventArgs)value;

            int rowIndex = args.Row.GetIndex();

            return new LoadListViewItemsCommandParameter(rowIndex, (DynamicListViewModel)parameter);
        }
    }
}
