namespace Infrastructure.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Input;
    using Infrastructure.Entities;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.ListViews;
    using Infrastructure.Reflection;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;

    public class DynamicListViewModel : TitledViewModel, INavigationAware, ISubscribeTo<LanguageChanged>
    {
        private readonly LocalizedString title;
        private readonly ListViewName listViewName;
        private readonly IListViewService listViewService;
        private readonly ILoggerFacade logger;
        private readonly ICommand loadListViewItemsCommand;
        private readonly ObservableCollection<FacetedViewModel> items;
        private readonly List<Facet> facets;
        private FilterViewModel filter;
        private bool isCurrentlyLoading;
        private int loadNextBeforeEndOfItemsThreshold;
        private int takeCount;

        public DynamicListViewModel(ListViewName listViewName, ResourceAccessor title, IListViewService listViewService, ILoggerFacade logger, ICommand loadListViewItemsCommand)
        {
            Contract.Requires(listViewService != null);

            this.title = new LocalizedString(() => this.Title, title.GetResource, this.OnPropertyChanged);
            this.listViewName = listViewName;
            this.listViewService = listViewService;
            this.logger = logger;
            this.loadListViewItemsCommand = loadListViewItemsCommand;
            this.items = new ObservableCollection<FacetedViewModel>();
            this.facets = new List<Facet>();
            this.filter = new EmptyFilterViewModel();

            this.TakeCount = 25;
            this.LoadNextBeforeEndOfItemsThreshold = 10;
        }

        public override string Title
        {
            get { return this.title.Value; }
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
        
        public ICommand LoadListViewItemsCommand
        {
            get { return this.loadListViewItemsCommand; }
        }

        public bool IsCurrentlyLoading
        {
            get
            {
                return this.isCurrentlyLoading;
            }

            set
            {
                if (this.isCurrentlyLoading != value)
                {
                    this.OnPropertyChanging(() => this.IsCurrentlyLoading);
                    this.isCurrentlyLoading = value;
                    this.OnPropertyChanged(() => this.IsCurrentlyLoading);
                }
            }
        }

        public int TakeCount
        {
            get
            {
                return this.takeCount;
            }

            set
            {
                if (this.takeCount != value)
                {
                    this.OnPropertyChanging(() => this.TakeCount);
                    this.takeCount = value;
                    this.OnPropertyChanged(() => this.TakeCount);
                }
            }
        }

        public int LoadNextBeforeEndOfItemsThreshold
        {
            get
            {
                return this.loadNextBeforeEndOfItemsThreshold;
            }

            set
            {
                if (this.loadNextBeforeEndOfItemsThreshold != value)
                {
                    this.OnPropertyChanging(() => this.LoadNextBeforeEndOfItemsThreshold);
                    this.loadNextBeforeEndOfItemsThreshold = value;
                    this.OnPropertyChanged(() => this.LoadNextBeforeEndOfItemsThreshold);
                }
            }
        }

        public void Handle(LanguageChanged message)
        {
            this.Refresh();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (this.Items.Count == 0)
            {
                this.LoadListViewItems();
            }
        }

        public void LoadListViewItems()
        {
            if (!this.IsCurrentlyLoading)
            {
                this.IsCurrentlyLoading = true;
                this.listViewService.BeginGetListView(this.ListViewName.ToString(), this.Items.Count, this.TakeCount, this.OnGetListViewCompleted, null);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ListViewName target = new ListViewName(navigationContext.Parameters["name"]);

            return this.ListViewName == target;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public string TranslatePropertyName(string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            Facet facet = this.facets.FirstOrDefault(f => string.Equals(propertyName, f.PropertyName, StringComparison.OrdinalIgnoreCase));

            if (facet != null)
            {
                return facet.GetResource();
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
            ListView listView = this.listViewService.EndGetListView(result);

            this.facets.Clear();

            this.facets.AddRange(listView.Properties.Select(p => new Facet
                {
                    PropertyName = p.PropertyName,
                    PropertyType = Type.GetType(p.PropertyType),
                    GetResource = this.CreateResourceAccessor(p.ResourceKey)
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

            this.IsCurrentlyLoading = false;
        }

        private Func<string> CreateResourceAccessor(string resourceIdentifier)
        {
            try
            {
                ResourceAccessor accessor = ResourceAccessor.Create(resourceIdentifier);

                return accessor.GetResource;
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    CultureInfo.CurrentCulture, 
                    "An error occured while trying to create an accessor for resource '{0}'.\r\n{1}", 
                    resourceIdentifier, 
                    ex);

                this.logger.Log(msg, Category.Exception, Priority.Medium);

                return () =>
                    {
                        string rid = resourceIdentifier.ToUpperInvariant();
                        return rid;
                    };
            }
        }
    }
}
