namespace TecX.Agile.Phone.ViewModels
{
    using System.Diagnostics;

    using Caliburn.Micro;

    using TecX.Agile.Phone.Data;

    public class MainPageViewModel
    {
        private readonly INavigationService navigationService;

        private readonly BindableCollection<Project> projects;
        
        public MainPageViewModel(INavigationService navigationService)
        {
            Guard.AssertNotNull(navigationService, "navigationService");

            this.navigationService = navigationService;
            this.projects = new BindableCollection<Project>();
            for(int i = 0; i < 100; i++)
            {
                this.projects.Add(new Project());
            }
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

        public void LoadMoreProjects(object parameter)
        {
            
        }
    }
}
