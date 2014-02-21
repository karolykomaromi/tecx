using Infrastructure.ViewModels;

namespace Infrastructure.Commands
{
    public class LoadListViewItemsCommandParameter
    {
        private readonly int rowIndex;
        private readonly DynamicListViewModel viewModel;

        public LoadListViewItemsCommandParameter(int rowIndex, DynamicListViewModel viewModel)
        {
            this.rowIndex = rowIndex;
            this.viewModel = viewModel;
        }

        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        public DynamicListViewModel ViewModel
        {
            get { return this.viewModel; }
        }
    }
}