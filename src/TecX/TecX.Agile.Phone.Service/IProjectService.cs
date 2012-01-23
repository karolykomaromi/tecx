namespace TecX.Agile.Phone.Service
{
    using System;
    using System.ServiceModel;

    [ServiceContract(Namespace = "http://tecx.codeplex.com/phone/project")]
    public interface IProjectService
    {
        [OperationContract]
        ProjectQueryResult GetProjects(int startingFromIndex, int takeCount);

        [OperationContract]
        IterationQueryResult GetIterations(int startingFromIndex, int takeCount, Guid projectId);

        [OperationContract]
        StoryQueryResult GetUserStories(int startingFromIndex, int takeCount, Guid iterationId);
    }
}
