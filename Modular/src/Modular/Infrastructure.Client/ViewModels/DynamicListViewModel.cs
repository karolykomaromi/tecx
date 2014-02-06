namespace Infrastructure.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Infrastructure.Entities;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.ListViews;
    using Microsoft.Practices.Prism.Regions;

    public class DynamicListViewModel : TitledViewModel, INavigationAware, ISubscribeTo<LanguageChanged>
    {
        private readonly ListViewName listViewName;
        private readonly IListViewService listViewService;
        private readonly ObservableCollection<FacetedViewModel> items;
        private readonly List<Facet> facets;
        private FilterViewModel filter;

        public DynamicListViewModel(ListViewName listViewName, ResxKey listViewTitleKey, IListViewService listViewService)
            : base(listViewTitleKey)
        {
            Contract.Requires(listViewService != null);

            this.listViewName = listViewName;
            this.listViewService = listViewService;
            this.items = new ObservableCollection<FacetedViewModel>();
            this.facets = new List<Facet>();
            this.filter = new EmptyFilterViewModel();
        }

        public ObservableCollection<FacetedViewModel> Items
        {
            get { return this.items; }
        }

        public ListViewName ListViewName
        {
            get { return this.listViewName; }
        }

        public FilterViewModel Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                if (this.filter != value)
                {
                    this.OnPropertyChanging(() => this.Filter);
                    this.filter = value;
                    this.OnPropertyChanged(() => this.Filter);
                }
            }
        }

        void ISubscribeTo<LanguageChanged>.Handle(LanguageChanged message)
        {
            this.Refresh();
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            this.listViewService.BeginGetListView(this.ListViewName.ToString(), 1, 50, this.OnGetListViewCompleted, null);
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            ListViewName target = new ListViewName(navigationContext.Parameters["name"]);

            return this.ListViewName == target;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        
        public string TranslatePropertyName(string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            Facet facet = this.facets.FirstOrDefault(f => string.Equals(propertyName, f.PropertyName, StringComparison.OrdinalIgnoreCase));

            if (facet != null)
            {
                return this.ResourceManager[facet.ResourceKey];
            }

            return propertyName;
        }

        public void Refresh()
        {
            List<FacetedViewModel> copy = new List<FacetedViewModel>(this.Items);

            this.Items.Clear();

            copy.ForEach(this.Items.Add);
        }

        private void OnGetListViewCompleted(IAsyncResult result)
        {
            this.Items.Clear();

            ListView listView = this.listViewService.EndGetListView(result);

            this.facets.Clear();

            this.facets.AddRange(listView.Properties.Select(p => new Facet
                {
                    PropertyName = p.PropertyName, 
                    PropertyType = Type.GetType(p.PropertyType), 
                    ResourceKey = new ResxKey(p.ResourceKey)
                }));

            foreach (ListViewRow row in listView.Rows)
            {
                FacetedViewModel vm = new FacetedViewModel();

                this.facets.ForEach(vm.AddFacet);

                foreach (ListViewCell cell in row.Cells)
                {
                    vm[cell.PropertyName] = cell.Value;
                }

                this.Items.Add(vm);
            }
        }
    }
}
