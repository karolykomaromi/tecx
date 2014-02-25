namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.Windows.Threading;
    using Infrastructure.Entities;

    public class ResourceServiceClient : ClientBase<IResourceService>, IResourceService
    {
        private readonly Dispatcher dispatcher;

        public ResourceServiceClient(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
        }

        public IAsyncResult BeginGetStrings(string moduleName, string culture, int skip, int take, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginGetStrings(moduleName, culture, skip, take, result => this.dispatcher.BeginInvoke(() => callback(result)), asyncState);
        }

        public ResourceString[] EndGetStrings(IAsyncResult result)
        {
            return this.Channel.EndGetStrings(result);
        }
    }
}