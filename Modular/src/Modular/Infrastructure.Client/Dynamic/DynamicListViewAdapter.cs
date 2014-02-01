namespace Infrastructure.Dynamic
{
    using Infrastructure.ViewModels;
    using Infrastructure.Views;

    public class DynamicListViewAdapter : UserControlAdapter
    {
        public DynamicListViewAdapter(DynamicListView listView, IControlAdapterFactory factory)
            : base(listView, factory)
        {
            DynamicListViewModel vm = listView.DataContext as DynamicListViewModel;
            
            if (vm != null)
            {
                string name = vm.ListViewName.ToString();

                this.Id = new ControlId(name);
            }
        }
    }
}
