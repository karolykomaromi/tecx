namespace TecX.Agile.Phone.Service
{
    using System.ServiceModel;

    [ServiceContract(Namespace = "http://tecx.codeplex.com/phone/project")]
    public interface IProjectService
    {
        [OperationContract]
        ProjectQueryResult GetProjects(int maxResultCount);

        [OperationContract]
        IterationQueryResult GetIterations(int maxResultCount, int projectId);

        [OperationContract]
        StoryQueryResult GetUserStories(int maxResultCount, int iterationId);
    }
}
