namespace TecX.Agile.Phone.Data
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;

    using TecX.Agile.Phone.ProjectService;

    public class DummyProjectApplicationService : IProjectApplicationService
    {
        public void GetProjectsAsync(int startingFromIndex, int takeCount, Action<ProjectQueryResult> callback)
        {
            Guard.AssertNotNull(callback, "callback");

            var projects = new ObservableCollection<Project>();

            var result = new ProjectQueryResult
                {
                    TotalResultCount = takeCount * 10,
                    Projects = projects
                };

            for (int i = 0; i < takeCount; i++)
            {
                string name = ((startingFromIndex + i) * 1000).ToString(CultureInfo.InvariantCulture);
                var prj = new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };

                projects.Add(prj);
            }

            callback(result);
        }

        public void GetIterationsAsync(int startingFromIndex, int takeCount, Guid projectId, Action<IterationQueryResult> callback)
        {
            Guard.AssertNotNull(callback, "callback");

            callback(new IterationQueryResult());
        }

        public void GetUserStoriesAsync(int startingFromIndex, int takeCount, Guid iterationId, Action<StoryQueryResult> callback)
        {
            Guard.AssertNotNull(callback, "callback");

            callback(new StoryQueryResult());
        }
    }
}
