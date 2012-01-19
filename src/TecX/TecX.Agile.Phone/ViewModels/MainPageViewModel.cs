namespace TecX.Agile.Phone.ViewModels
{
    using System.Diagnostics;

    using Caliburn.Micro;
    
    public class MainPageViewModel
    {
        private readonly INavigationService navigationService;
        
        public MainPageViewModel(INavigationService navigationService)
        {
            Guard.AssertNotNull(navigationService, "navigationService");

            this.navigationService = navigationService;
        }

        public void GotoPageTwo()
        {
            this.navigationService.UriFor<PivotPageViewModel>()
                .WithParam(x => x.NumberOfTabs, 5)
                .Navigate();

            //var x = this.projectService.BeginGetProjects(
            //    100,
            //    asyncResult =>
            //        {
            //            var svc = (IAsyncProjectService)asyncResult.AsyncState;
            //            var projectQueryResult = svc.EndGetProjects(asyncResult);
            //            Debug.WriteLine("call successfull");
            //        },
            //    this.projectService);
        }
    }
}
