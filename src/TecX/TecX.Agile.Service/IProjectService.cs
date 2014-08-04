namespace TecX.Agile.Service
{
    using System;
    using System.ServiceModel;

    using TecX.Agile.Service.Results;

    [ServiceContract(Namespace = "http://tecx.codeplex.com/phone/project")]
    public interface IProjectService
    {
        [OperationContract]
        QueryProjectsResult GetProjects(int startingFromIndex, int takeCount);

        [OperationContract]
        QueryIterationsResult GetIterations(int startingFromIndex, int takeCount, Guid projectId);

        [OperationContract]
        QueryStoriesResult GetUserStories(int startingFromIndex, int takeCount, Guid iterationId);

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
