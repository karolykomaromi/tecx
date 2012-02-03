namespace TecX.Agile.Service
{
    using System;
    using System.ServiceModel;

    using TecX.Agile.Service.Results;

    [ServiceContract(Namespace = "http://tecx.codeplex.com/phone/project")]
    public interface IProjectService
    {
        [OperationContract]
        ProjectQueryResult GetProjects(int startingFromIndex, int takeCount);

        [OperationContract]
        IterationQueryResult GetIterations(int startingFromIndex, int takeCount, Guid projectId);

        [OperationContract]
        StoryQueryResult GetUserStories(int startingFromIndex, int takeCount, Guid iterationId);

        [OperationContract]
        AddProjectResult AddProject(Project newProject);

        [OperationContract]
        AddIterationResult AddIteration(Iteration newIteration);

        [OperationContract]
        AddStoryResult AddStory(StoryCard newStory);

        [OperationContract]
        RemoveProjectResult RemoveProject(Guid projectId);

        [OperationContract]
        RemoveIterationResult RemoveIteration(Guid iterationId);

        [OperationContract]
        RemoveStoryResult RemoveStory(Guid storyId);
    }
}
