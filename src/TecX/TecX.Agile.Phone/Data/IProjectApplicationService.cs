namespace TecX.Agile.Phone.Data
{
    using System;

    using TecX.Agile.Phone.ProjectService;

    public interface IProjectApplicationService
    {
        void GetProjectsAsync(int startingFromIndex, int takeCount, Action<ProjectQueryResult> callback);

        void GetIterationsAsync(int startingFromIndex, int takeCount, Guid projectId, Action<IterationQueryResult> callback);

        void GetUserStoriesAsync(int startingFromIndex, int takeCount, Guid iterationId, Action<StoryQueryResult> callback);
    }
}