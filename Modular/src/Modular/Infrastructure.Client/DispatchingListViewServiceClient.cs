namespace Infrastructure
{
    using System;
    using System.Windows.Threading;
    using Infrastructure.Entities;

    public class DispatchingListViewServiceClient : IListViewService
    {
        private readonly IListViewService inner;

        private readonly Dispatcher dispatcher;

        public DispatchingListViewServiceClient(IListViewService inner, Dispatcher dispatcher)
        {
            this.inner = inner;
            this.dispatcher = dispatcher;
        }

        public IAsyncResult BeginGetListView(string listViewName, int skip, int take, AsyncCallback callback, object asyncState)
        {
            return this.inner.BeginGetListView(listViewName, skip, take, new Dispatched(this.dispatcher, callback).Callback, asyncState);
        }

        public ListView EndGetListView(IAsyncResult result)
        {
            return this.inner.EndGetListView(result);
        }

        private class Dispatched
        {
            private readonly Dispatcher dispatcher;

            private readonly AsyncCallback callback;

            public Dispatched(Dispatcher dispatcher, AsyncCallback callback)
            {
                this.dispatcher = dispatcher;
                this.callback = callback;
            }

            public void Callback(IAsyncResult result)
            {
                this.dispatcher.BeginInvoke(() => this.callback(result));
            }
        }
    }
}