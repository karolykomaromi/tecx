using System;

using TecX.Agile.ViewModel.Messages;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel.Remote
{
    public class RemoteUI : IDisposable,
        ISubscribeTo<PropertyChanged>,
        ISubscribeTo<StoryCardRescheduled>,
        ISubscribeTo<StoryCardPostponed>
    {
        public RemoteUI()
        {
            HighlightEventHub.FieldHighlighted += OnFieldHighlighted;
        }

        private void OnFieldHighlighted(object sender, HighlightEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                HighlightEventHub.FieldHighlighted -= OnFieldHighlighted;
            }
        }

        #region Implementation of IRemoteUI

        public void Handle(PropertyChanged message)
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
