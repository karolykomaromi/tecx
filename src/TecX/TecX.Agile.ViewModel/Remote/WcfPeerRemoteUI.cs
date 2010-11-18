using System;

using TecX.Agile.Peer;
using TecX.Agile.ViewModel.Messages;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel.Remote
{
    public class WcfPeerRemoteUI : IDisposable,
        ISubscribeTo<PropertyChanged>,
        ISubscribeTo<StoryCardRescheduled>,
        ISubscribeTo<StoryCardPostponed>
    {
        #region Fields

        private readonly IPeerClient _peerClient;

        #endregion Fields

        #region c'tor

        public WcfPeerRemoteUI(IPeerClient peerClient)
        {
            Guard.AssertNotNull(peerClient, "peerClient");

            _peerClient = peerClient;

            RemoteHighlight.OutgoingNotificationThatFieldWasHighlighted += OnLocalOutgoingNotificationThatFieldWasHighlighted;

            _peerClient.FieldHighlighted += OnRemoteFieldHighlighted;
        }

        #endregion c'tor

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoteHighlight.OutgoingNotificationThatFieldWasHighlighted -= OnLocalOutgoingNotificationThatFieldWasHighlighted;
            }
        }

        #endregion Implementation of IDisposable

        private void OnLocalOutgoingNotificationThatFieldWasHighlighted(object sender, RemoteHighlightEventArgs e)
        {
            _peerClient.Highlight(_peerClient.Id, e.ArtefactId, e.FieldName);
        }

        private void OnRemoteFieldHighlighted(object sender, Peer.FieldHighlightedEventArgs e)
        {
            Guard.AssertNotNull(e, "e");
            Guard.AssertNotEmpty(e.FieldName, "e.FieldName");

            RemoteHighlight.RaiseIncomingRequestToHighlightField(this, new RemoteHighlightEventArgs(e.ArtefactId, e.FieldName));
        }

        public void Handle(PropertyChanged message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotNull(message.ParentObject, "message.ParentObject");
            Guard.AssertNotEmpty(message.PropertyName, "message.PropertyName");

            _peerClient.UpdateProperty(_peerClient.Id, message.ParentObject.Id, message.PropertyName, message.NewValue);
        }

        public void Handle(StoryCardRescheduled message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardPostponed message)
        {
            throw new NotImplementedException();
        }
    }
}
