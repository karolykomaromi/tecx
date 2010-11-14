using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Agile.ViewModel.ChangeTracking;
using TecX.Agile.ViewModel.Messages;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel.Remote
{
    public class RemoteUI : IRemoteUI, 
        ISubscribeTo<PropertyChanged>,
        ISubscribeTo<CollectionChanged<StoryCard>>,
        ISubscribeTo<CollectionChanged<Iteration>>,
        ISubscribeTo<StoryCardRescheduled>,
        ISubscribeTo<StoryCardPostponed>
    {
        #region Implementation of IRemoteUI

        public void HighlightField(Guid artefactId, string fieldName)
        {
            throw new NotImplementedException();
        }

        public void UpdateProperty(Guid artefactId, string propertyName, object newValue)
        {
            throw new NotImplementedException();
        }

        public void Handle(PropertyChanged message)
        {
            throw new NotImplementedException();
        }

        public void Handle(CollectionChanged<StoryCard> message)
        {
            throw new NotImplementedException();
        }

        public void Handle(CollectionChanged<Iteration> message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardRescheduled message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardPostponed message)
        {
            throw new NotImplementedException();
        }

        #endregion Implementation of IRemoteUI
    }
}
