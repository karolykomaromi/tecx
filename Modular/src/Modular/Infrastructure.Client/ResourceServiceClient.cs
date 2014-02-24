using System;
using System.ServiceModel;
using System.Windows.Threading;
using Infrastructure.Entities;
using Infrastructure.I18n;

namespace Infrastructure
{
    public class ResourceServiceClient : ClientBase<IResourceService>, IResourceService
    {
        private readonly Dispatcher dispatcher;

        public ResourceServiceClient(Dispatcher dispatcher)
        {
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