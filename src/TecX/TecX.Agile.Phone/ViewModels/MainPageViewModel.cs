namespace TecX.Agile.Phone.ViewModels
{
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;

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
            this.projects = new BindableCollection<Project>();
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

            this.projectService.GetProjectsAsync(
                this.Projects.Count,
                Constants.LoadBatchSize,
                result =>
                {
                    Guard.AssertNotNull(result, "result");
                    Guard.AssertNotNull(result.Projects, "result.Projects");

                    this.Projects.AddRange(result.Projects);
                });
        }

        public void LoadMoreProjects(ScrollChangedParameter parameter)
        {
            //TODO weberse 2012-01-23 check how far down the list we are and only load if we are close to the end

            //Application.Current.ApplicationLifetimeObjects.OfType<IProjectApplicationService>().Single() ...

            //var x = this.projectService.BeginGetProjects(
            //    100,
            //    asyncResult =>
            //        {
            //            var svc = (IAsyncProjectService)asyncResult.AsyncState;
            //            var projectQueryResult = svc.EndGetProjects(asyncResult);
            //            Debug.WriteLine("call successfull");
            //        },
            //    this.projectService);

            if (this.Projects.Count == 0)
            {
                return;
            }

            //double totalHeight = parameter.ExtentHeight;
            //double heightPerItem = totalHeight / this.Projects.Count;

            //double remainingHeight = totalHeight - (parameter.VerticalOffset + parameter.ViewportHeight);

            //double remainingItems = remainingHeight / heightPerItem;

            //if (remainingItems < (Constants.LoadBatchSize / 2))
            //{

            if((parameter.VerticalOffset + parameter.ViewportHeight) >= parameter.ExtentHeight)
            {
                this.projectService.GetProjectsAsync(
                    this.Projects.Count,
                    Constants.LoadBatchSize,
                    result =>
                    {
                        Guard.AssertNotNull(result, "result");
                        Guard.AssertNotNull(result.Projects, "result.Projects");

                        //TODO weberse 2012-01-23 the listbox seems to loose the binding if I add items here.
                        this.Projects.AddRange(result.Projects);
                    });
            }
        }
    }
}
