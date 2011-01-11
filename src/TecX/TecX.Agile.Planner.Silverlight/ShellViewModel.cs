using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Threading;

using TecX.Agile.ChangeTracking;
using TecX.Agile.Infrastructure;
using TecX.Agile.Remote;
using TecX.Agile.Serialization;
using TecX.Agile.Serialization.Messages;
using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Planner
{
    public class ShellViewModel
    {
        #region Fields

        private readonly IChangeTracker _changeTracker;
        private readonly Func<IRemoteUI> _remoteUIFactory;
        private IRemoteUI _remoteUI;
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

        public ShellViewModel(IChangeTracker changeTracker, Func<IRemoteUI> remoteUIFactory,
            EventAggregatorAccessor eventAggregatorAccessor)
        {
            Guard.AssertNotNull(changeTracker, "changeTracker");
            Guard.AssertNotNull(remoteUIFactory, "remoteUIFactory");

            _changeTracker = changeTracker;
            _remoteUIFactory = remoteUIFactory;

            //TODO weberse initialization of current project must be moved somewhere else
            CurrentProject = new Project { Id = Guid.NewGuid() };
        }

        #endregion c'tor

        public void InitializeConnection()
        {
            _remoteUI = _remoteUIFactory();
        }
    }
}
