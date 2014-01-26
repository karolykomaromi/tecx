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

    public class DynamicListViewModel : ViewModel, ISubscribeTo<OptionsChanged>, INavigationAware
    {
        private readonly ListViewName listViewName;
        private readonly LocalizedString title;
        private readonly IListViewService listViewService;

        private readonly ObservableCollection<FacetedViewModel> items;

        public DynamicListViewModel(ListViewName listViewName, ResxKey listViewTitleKey, IListViewService listViewService)
        {
            Contract.Requires(listViewTitleKey != ResxKey.Empty);
            Contract.Requires(listViewService != null);

            this.listViewName = listViewName;
            this.listViewService = listViewService;
            this.title = new LocalizedString(this, "Title", listViewTitleKey, this.OnPropertyChanged);
            this.items = new ObservableCollection<FacetedViewModel>();
        }

        public string Title
        {
            get
            {
                return this.title.Value;
            }
        }

        public ObservableCollection<FacetedViewModel> Items
        {
            get
            {
                return this.items;
            }
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
            ListViewName target = new ListViewName(navigationContext.Uri.ToString());

            return this.listViewName == target;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void OnGetListViewCompleted(IAsyncResult result)
        {
            ListView listView = this.listViewService.EndGetListView(result);

            // can be done here because service marshalls callback to ui thread
        }
    }
}
