namespace Infrastructure.ListViews
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.Windows.Threading;
    using Infrastructure.Entities;

    public class ListViewServiceClient : ClientBase<IListViewService>, IListViewService
    {
        private readonly Dispatcher dispatcher;

        public ListViewServiceClient(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
        }

        public IAsyncResult BeginGetListView(string listViewName, int skip, int take, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginGetListView(listViewName, skip, take, result => this.dispatcher.BeginInvoke(() => callback(result)), asyncState);
        }

        public ListView EndGetListView(IAsyncResult result)
        {
            return this.Channel.EndGetListView(result);
        }
    }
}