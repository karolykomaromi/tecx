using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Agile.ViewModel.ChangeTracking;
using TecX.Agile.ViewModel.Remote;
using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class ShellViewModel
    {
        private readonly IRemoteUI _remoteUI;
        private readonly IChangeTracker _changeTracker;

        public ShellViewModel(IRemoteUI remoteUI, IChangeTracker changeTracker, EventAggregatorAccessor eventAggregatorAccessor)
        {
            Guard.AssertNotNull(remoteUI, "remoteUI");
            Guard.AssertNotNull(changeTracker, "changeTracker");

            _remoteUI = remoteUI;
            _changeTracker = changeTracker;
        }

        //TODO weberse i want to hook up some info object that gives you current application version, deployment method (click once etc.),
        //product name, versions of all assemblies in current appdomain and the like

        private StoryCard _card;

        public StoryCard Card
        {
            get { return _card; }
            set
            {
                _changeTracker.Unsubscribe(_card);
                
                _card = value;
                
                _changeTracker.Subscribe(_card);
            }
        }
    }
}
