namespace TecX.Agile.Phone.ViewModels
{
    using System.Diagnostics;

    using Caliburn.Micro;

    using TecX.Agile.Phone.Service;

    public class MainPageViewModel
    {
        private readonly INavigationService navigationService;

        private readonly IAsyncProjectService projectService;

        public MainPageViewModel(INavigationService navigationService, IAsyncProjectService projectService)
        {
            Guard.AssertNotNull(navigationService, "navigationService");
            Guard.AssertNotNull(projectService, "projectService");

            this.navigationService = navigationService;
            this.projectService = projectService;
        }

        public void GotoPageTwo()
        {
            //this.navigationService.UriFor<PivotPageViewModel>()
            //    .WithParam(x => x.NumberOfTabs, 5)
            //    .Navigate();

            var x = this.projectService.BeginGetProjects(
                100,
                asyncResult =>
                    {
                        var svc = (IAsyncProjectService)asyncResult.AsyncState;
                        var projectQueryResult = svc.EndGetProjects(asyncResult);
                        Debug.WriteLine("call successfull");
                    },
                this.projectService);
        }
    }
}
