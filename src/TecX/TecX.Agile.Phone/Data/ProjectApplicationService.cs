namespace TecX.Agile.Phone.Data
{
    using System;
    using System.Windows;

    using TecX.Agile.Phone.ProjectService;

    public class ProjectApplicationService : IApplicationService, IProjectApplicationService
    {
        public void GetProjectsAsync(int startingFromIndex, int takeCount, Action<ProjectQueryResult> callback)
        {
            var proxy = new ProjectServiceClient();

            proxy.GetProjectsCompleted += (s, e) => callback(e.Result);

            proxy.GetProjectsAsync(startingFromIndex, takeCount);
        }

        public void GetIterationsAsync(int startingFromIndex, int takeCount, Guid projectId, Action<IterationQueryResult> callback)
        {
            var proxy = new ProjectServiceClient();

            proxy.GetIterationsCompleted += (s, e) => callback(e.Result);

            proxy.GetIterationsAsync(startingFromIndex, takeCount, projectId);
        }

        public void GetUserStoriesAsync(int startingFromIndex, int takeCount, Guid iterationId, Action<StoryQueryResult> callback)
        {
            var proxy = new ProjectServiceClient();

            proxy.GetUserStoriesCompleted += (s, e) => callback(e.Result);

            proxy.GetUserStoriesAsync(startingFromIndex, takeCount, iterationId);
        }

        public void StartService(ApplicationServiceContext context)
        {
        }

        public void StopService()
        {
        }
    }
}
