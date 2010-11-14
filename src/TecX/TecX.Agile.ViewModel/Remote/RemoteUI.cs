using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Agile.ViewModel.ChangeTracking;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel.Remote
{
    public class RemoteUI : IRemoteUI, 
        ISubscribeTo<PropertyChanged>,
        ISubscribeTo<CollectionChanged<StoryCard>>,
        ISubscribeTo<CollectionChanged<Iteration>>,
        ISubscribeTo<RescheduledStoryCard>
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

        #endregion

        #region Implementation of ISubscribeTo<in PropertyChanged>

        public void Handle(PropertyChanged message)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of ISubscribeTo<in CollectionChanged<StoryCard>>

        public void Handle(CollectionChanged<StoryCard> message)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of ISubscribeTo<in CollectionChanged<Iteration>>

        public void Handle(CollectionChanged<Iteration> message)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of ISubscribeTo<in RescheduledStoryCard>

        public void Handle(RescheduledStoryCard message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
