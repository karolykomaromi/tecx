namespace TecX.Agile.Phone.Service
{
    using System;
    using System.ServiceModel;

    [ServiceContract(Namespace = "http://tecx.codeplex.com/phone/project", Name = "IProjectService")]
    public interface IAsyncProjectService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetProjects(int maxResultCount, AsyncCallback callback, object userState);

        ProjectQueryResult EndGetProjects(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetIterations(int maxResultCount, int projectId, AsyncCallback callback, object userState);

        IterationQueryResult EndGetIterations(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetUserStories(int maxResultCount, int iterationId, AsyncCallback callback, object userState);

        StoryQueryResult EndGetUserStories(IAsyncResult result);
    }
}