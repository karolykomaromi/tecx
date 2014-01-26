namespace Infrastructure.ListViews
{
    using System.Diagnostics.Contracts;
    using System.Windows.Threading;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;

    public class ListViewRegistry
    {
        private readonly IListViewService service;
        private readonly Dispatcher dispatcher;

        public ListViewRegistry(IListViewService service, Dispatcher dispatcher)
        {
            Contract.Requires(service != null);
            Contract.Requires(dispatcher != null);

            this.service = service;
            this.dispatcher = dispatcher;
        }

        public void Add(ListViewName listViewName, ResxKey listViewTitleKey)
        {
            DynamicListViewModel vm = null;

            vm = new DynamicListViewModel(
                listViewName,
                listViewTitleKey,
                this.service);
        }
    }
}