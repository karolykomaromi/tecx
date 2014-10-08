namespace TecX.Agile.Phone.ViewModels
{
    using Caliburn.Micro;

    using TecX.Agile.Phone.Data;
    using TecX.Agile.Phone.ProjectService;
    using TecX.CaliburnEx;

    public class MainPageViewModel
    {
        private static class Constants
        {
            public const int LoadBatchSize = 25;
        }

        private readonly INavigationService navigationService;

        private readonly IProjectApplicationService projectService;

        private readonly BindableCollection<Project> projects;

        public MainPageViewModel(INavigationService navigationService, IProjectApplicationService projectService)
        {
            Guard.AssertNotNull(navigationService, "navigationService");
            Guard.AssertNotNull(projectService, "projectService");

            this.navigationService = navigationService;
            this.projectService = projectService;
            this.projects = new BindableCollection<Project> { IsNotifying = true };
        }

        public IObservableCollection<Project> Projects
        {
            get
            {
                return this.projects;
            }
        }

        public void GotoPageTwo()
        {
            //this.navigationService.UriFor<PivotPageViewModel>()
            //    .WithParam(x => x.NumberOfTabs, 5)
            //    .Navigate();

            this.GetNextProjects();
        }

        public void LoadMoreProjects(ScrollChangedParameter parameter)
        {
            //TODO weberse 2012-01-23 check how far down the list we are and only load if we are close to the end
            if (this.Projects.Count == 0)
            {
                return;
            }

            double scrollableHeight = parameter.ExtentHeight - parameter.ViewportHeight;

            if (parameter.VerticalOffset >= scrollableHeight)
            {
                this.GetNextProjects();
            }
        }

        private void GetNextProjects()
        {
            this.projectService.GetProjectsAsync(
                this.Projects.Count,
                Constants.LoadBatchSize,
                result =>
                {
                    Guard.AssertNotNull(result, "result");
                    Guard.AssertNotNull(result.Projects, "result.Projects");

                    result.Projects.ForEach(p => this.Projects.Add(p));
                });
        }
    }
}
