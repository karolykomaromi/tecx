namespace TecX.Agile.Phone.Data
{
    using System;
    using System.Windows;

    using TecX.Agile.Phone.ProjectService;

    public class ProjectApplicationService : IApplicationService
    {
        public void GetProjectsAsync(int maxResultCount, Action<ProjectQueryResult> callback)
        {
            var proxy = new ProjectServiceClient();

            proxy.GetProjectsCompleted += (s, e) => callback(e.Result);

            proxy.GetProjectsAsync(maxResultCount);
        }

        public void GetIterationsAsync(int maxResultCount, int projectId, Action<IterationQueryResult> callback)
        {
            var proxy = new ProjectServiceClient();

            proxy.GetIterationsCompleted += (s, e) => callback(e.Result);

            proxy.GetIterationsAsync(maxResultCount, projectId);
        }

        public void GetUserStoriesAsync(int maxResultCount, int iterationId, Action<StoryQueryResult> callback)
        {
            var proxy = new ProjectServiceClient();

            proxy.GetUserStoriesCompleted += (s, e) => callback(e.Result);

            proxy.GetUserStoriesAsync(maxResultCount, iterationId);
        }

        public void StartService(ApplicationServiceContext context)
        {
        }

        public void StopService()
        {
        }
    }
}
