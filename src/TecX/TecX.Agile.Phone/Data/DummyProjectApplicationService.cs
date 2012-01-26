namespace TecX.Agile.Phone.Data
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows;

    using TecX.Agile.Phone.ProjectService;

    public class DummyProjectApplicationService : IProjectApplicationService
    {
        private readonly IDispatcher dispatcher;

        public DummyProjectApplicationService(IDispatcher dispatcher)
        {
            Guard.AssertNotNull(dispatcher, "dispatcher");

            this.dispatcher = dispatcher;
        }

        public void GetProjectsAsync(int startingFromIndex, int takeCount, Action<ProjectQueryResult> callback)
        {
            if(startingFromIndex == takeCount * 10)
            {
                this.dispatcher.BeginInvoke(
                    () => callback(new ProjectQueryResult
                                    {
                                        TotalResultCount = 250, 
                                        Projects = new ObservableCollection<Project>()
                                    }));
            }

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

            this.dispatcher.BeginInvoke(() => callback(result));
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
