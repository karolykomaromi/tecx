namespace Infrastructure.Dynamic
{
    using Infrastructure.ViewModels;
    using Infrastructure.Views;

    public class NavigationViewAdapter : UserControlAdapter
    {
        public NavigationViewAdapter(NavigationView navigationView, IControlAdapterFactory factory)
            : base(navigationView, factory)
        {
            NavigationViewModel vm = navigationView.DataContext as NavigationViewModel;

            if (vm != null)
            {
                this.Id = new ControlId(vm.Destination.ToString());
            }
        }
    }
}