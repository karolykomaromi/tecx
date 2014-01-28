namespace Infrastructure.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Infrastructure.Entities;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.ListViews;
    using Infrastructure.Options;
    using Microsoft.Practices.Prism.Regions;

    public class DynamicListViewModel : TitledViewModel, ISubscribeTo<OptionsChanged>, INavigationAware
    {
        private readonly ListViewName listViewName;
        private readonly IListViewService listViewService;
        private readonly ObservableCollection<FacetedViewModel> items;

        public DynamicListViewModel(ListViewName listViewName, ResxKey listViewTitleKey, IListViewService listViewService)
            : base(listViewTitleKey)
        {
            Contract.Requires(listViewService != null);

            this.listViewName = listViewName;
            this.listViewService = listViewService;
            this.items = new ObservableCollection<FacetedViewModel>();
        }

        public ObservableCollection<FacetedViewModel> Items
        {
            get { return this.items; }
        }

        void ISubscribeTo<OptionsChanged>.Handle(OptionsChanged message)
        {
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            this.listViewService.BeginGetListView(this.listViewName.ToString(), 1, 50, this.OnGetListViewCompleted, null);
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            ListViewName target = new ListViewName(navigationContext.Parameters["name"]);

            return this.listViewName == target;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void OnGetListViewCompleted(IAsyncResult result)
        {
            this.Items.Clear();

            ListView listView = this.listViewService.EndGetListView(result);

            foreach (ListViewRow row in listView.Rows)
            {
                FacetedViewModel vm = new FacetedViewModel();

                foreach (Property property in listView.Properties)
                {
                    vm.AddFacet(new Facet { PropertyName = property.PropertyName, PropertyType = Type.GetType(property.PropertyType) });
                }

                foreach (ListViewCell cell in row.Cells)
                {
                    vm[cell.PropertyName] = cell.Value;
                }

                this.Items.Add(vm);
            }
        }
    }
}
