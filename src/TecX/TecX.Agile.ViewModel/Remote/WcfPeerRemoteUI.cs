using System;

using TecX.Agile.Peer;
using TecX.Agile.ViewModel.Messages;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel.Remote
{
    public class WcfPeerRemoteUI : IRemoteUI
    {
        #region Fields

        private readonly IPeerClient _peerClient;
        private readonly IEventAggregator _eventAggregator;

        #endregion Fields

        #region c'tor

        public WcfPeerRemoteUI(IPeerClient peerClient, IEventAggregator eventAggregator, PeerServiceHost host)
        {
            Guard.AssertNotNull(peerClient, "peerClient");
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _peerClient = peerClient;
            _eventAggregator = eventAggregator;

            _peerClient.IncomingRequestToHighlightField += OnIncomingRequestToHighlightIncomingRequestToHighlightField;
        }

        #endregion c'tor

        #region EventAggregator Subscriptions

        public void Handle(PropertyChanged message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotNull(message.ParentObject, "message.ParentObject");
            Guard.AssertNotEmpty(message.PropertyName, "message.PropertyName");

            _peerClient.UpdateProperty(_peerClient.Id, message.ParentObject.Id, message.PropertyName, message.NewValue);
        }

        public void Handle(StoryCardRescheduled message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotNull(message.From, "message.From");
            Guard.AssertNotNull(message.To, "message.To");
            Guard.AssertNotNull(message.StoryCard, "message.StoryCard");

            throw new NotImplementedException();
        }

        public void Handle(StoryCardPostponed message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotNull(message.From, "message.From");
            Guard.AssertNotNull(message.StoryCard, "message.StoryCard");

            throw new NotImplementedException();
        }

        public void Handle(OutgoingNotificationToHighlightField message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.FieldName, "message.FieldName");

            _peerClient.Highlight(_peerClient.Id, message.ArtefactId, message.FieldName);
        }

        #endregion EventAggregator Subscriptions

        #region EventHandler

        private void OnIncomingRequestToHighlightIncomingRequestToHighlightField(object sender, FieldHighlightedEventArgs e)
        {
            Guard.AssertNotNull(e, "e");
            Guard.AssertNotEmpty(e.FieldName, "e.FieldName");

            _eventAggregator.Publish(new IncomingRequestToHighlightField(e.ArtefactId, e.FieldName));
        }

        #endregion EventHandler
    }
}
