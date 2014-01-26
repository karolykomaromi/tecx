namespace Infrastructure
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Infrastructure.Entities;

    public class ListViewServiceClient : ClientBase<IListViewService>, IListViewService
    {
        public ListViewServiceClient()
        {
        }

        public ListViewServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public ListViewServiceClient(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public ListViewServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public ListViewServiceClient(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public IAsyncResult BeginGetListView(string listViewName, int pageNumber, int pageSize, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginGetListView(listViewName, pageNumber, pageSize, callback, asyncState);
        }

        public ListView EndGetListView(IAsyncResult result)
        {
            return this.Channel.EndGetListView(result);
        }
    }
}