namespace TecX.Agile.Phone.Data
{
    using System;
    using System.ServiceModel;

    using TecX.Agile.Phone.Service;

    public class ProjectServiceClient : ClientBase<IAsyncProjectService>, IAsyncProjectService
    {
        public ProjectServiceClient()
        {
        }

        public ProjectServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public ProjectServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public ProjectServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public ProjectServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public IAsyncResult BeginGetProjects(int maxResultCount, AsyncCallback callback, object userState)
        {
            return this.Channel.BeginGetProjects(maxResultCount, callback, userState);
        }

        public ProjectQueryResult EndGetProjects(IAsyncResult result)
        {
            return this.Channel.EndGetProjects(result);
        }

        public IAsyncResult BeginGetIterations(int maxResultCount, int projectId, AsyncCallback callback, object userState)
        {
            return this.Channel.BeginGetIterations(maxResultCount, projectId, callback, userState);
        }

        public IterationQueryResult EndGetIterations(IAsyncResult result)
        {
            return this.Channel.EndGetIterations(result);
        }

        public IAsyncResult BeginGetUserStories(int maxResultCount, int iterationId, AsyncCallback callback, object userState)
        {
            return this.Channel.BeginGetUserStories(maxResultCount, iterationId, callback, userState);
        }

        public StoryQueryResult EndGetUserStories(IAsyncResult result)
        {
            return this.Channel.EndGetUserStories(result);
        }

        protected override IAsyncProjectService CreateChannel()
        {
            return new ProjectServiceChannel(this);
        }

        private class ProjectServiceChannel : ChannelBase<IAsyncProjectService>, IAsyncProjectService
        {
            public ProjectServiceChannel(ClientBase<IAsyncProjectService> client)
                : base(client)
            {
            }

            public IAsyncResult BeginGetProjects(int maxResultCount, AsyncCallback callback, object userState)
            {
                object[] args = new object[] { maxResultCount };

                var result = this.BeginInvoke("GetProjects", args, callback, userState);

                return result;
            }

            public ProjectQueryResult EndGetProjects(IAsyncResult result)
            {
                object[] args = new object[0];

                ProjectQueryResult queryResult = (ProjectQueryResult)this.EndInvoke("GetProjects", args, result);

                return queryResult;
            }

            public IAsyncResult BeginGetIterations(int maxResultCount, int projectId, AsyncCallback callback, object userState)
            {
                object[] args = new object[] { maxResultCount, projectId };

                var result = this.BeginInvoke("GetIterations", args, callback, userState);

                return result;
            }

            public IterationQueryResult EndGetIterations(IAsyncResult result)
            {
                object[] args = new object[0];

                IterationQueryResult queryResult = (IterationQueryResult)this.EndInvoke("GetIterations", args, result);

                return queryResult;
            }

            public IAsyncResult BeginGetUserStories(int maxResultCount, int iterationId, AsyncCallback callback, object userState)
            {
                object[] args = new object[] { maxResultCount, iterationId };

                var result = this.BeginInvoke("GetUserStories", args, callback, userState);

                return result;
            }

            public StoryQueryResult EndGetUserStories(IAsyncResult result)
            {
                object[] args = new object[0];

                StoryQueryResult queryResult = (StoryQueryResult)this.EndInvoke("GetUserStories", args, result);

                return queryResult;
            }
        }
    }
}
