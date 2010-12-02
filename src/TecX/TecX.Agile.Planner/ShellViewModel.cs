using System;

using TecX.Agile.ChangeTracking;
using TecX.Agile.Infrastructure;
using TecX.Agile.Remote;
using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Planner
{
    //TODO weberse i want to hook up some info object that gives you current application version, deployment method (click once etc.),
    //product name, versions of all assemblies in current appdomain and the like
    public class ShellViewModel
    {
        #region Fields

        private readonly IRemoteUI _remoteUI;
        private readonly IChangeTracker _changeTracker;
        private Project _currentProject;
        private StoryCard _card;

        #endregion Fields

        #region Properties

        public StoryCard Card
        {
            get { return _card; }
            set
            {
                if (_card != null)
                    _changeTracker.Unsubscribe(_card);

                _card = value;

                _changeTracker.Subscribe(_card);
            }
        }

        public Project CurrentProject
        {
            get { return _currentProject; }
            set
            {
                Guard.AssertNotNull(value, "value");

                if (_currentProject == value)
                    return;

                if (_currentProject != null)
                    _changeTracker.Unsubscribe(_currentProject);

                _currentProject = value;

                _changeTracker.Subscribe(_currentProject);
            }
        }

        #endregion Properties

        #region c'tor

        public ShellViewModel(IRemoteUI remoteUI, IChangeTracker changeTracker, EventAggregatorAccessor eventAggregatorAccessor)
        {
            Guard.AssertNotNull(remoteUI, "remoteUI");
            Guard.AssertNotNull(changeTracker, "changeTracker");

            _remoteUI = remoteUI;
            _changeTracker = changeTracker;

            //TODO weberse initialization of current project must be moved somewhere else
            CurrentProject = new Project { Id = Guid.NewGuid() };
        }

        #endregion c'tor
    }
}
