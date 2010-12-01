using System;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;
using TecX.Common;

namespace TecX.Agile.Remote
{
    public class WcfPeerRemoteUI : IRemoteUI
    {
        #region Fields

        private readonly IPeerClient _peerClient;
        private readonly HighlightMessageFilter _highlightMessageFilter;
        private readonly PropertyChangedMessageFilter _propertyChangedMessageFilter;

        #endregion Fields

        #region c'tor

        public WcfPeerRemoteUI(IPeerClient peerClient)
        {
            Guard.AssertNotNull(peerClient, "peerClient");

            _peerClient = peerClient;

            _peerClient.IncomingRequestToHighlightField += OnIncomingRequestToHighlightField;
            _peerClient.PropertyUpdated += OnPropertyUpdated;

            _highlightMessageFilter = new HighlightMessageFilter();
            _propertyChangedMessageFilter = new PropertyChangedMessageFilter();
        }


        #endregion c'tor

        #region EventAggregator Subscriptions

        public void Handle(PropertyChanged message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.PropertyName, "message.PropertyName");

            if (_propertyChangedMessageFilter.ShouldLetPass(message))
            {
                _peerClient.UpdateProperty(_peerClient.Id, 
                    message.ArtefactId, message.PropertyName, message.OldValue, message.NewValue);
            }
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

        public void Handle(FieldHighlighted message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.FieldName, "message.FieldName");

            if(_highlightMessageFilter.ShouldLetPass(message))
            {
                _peerClient.Highlight(_peerClient.Id, message.ArtefactId, message.FieldName);
            }
        }

        public void Handle(StoryCardMoved message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotNull(message.StoryCard, "message.StoryCard");

            //TODO weberse filter messages!

            _peerClient.MoveStoryCard(_peerClient.Id, message.StoryCard.Id, message.X, message.Y, message.Angle);
        }

        #endregion EventAggregator Subscriptions

        #region EventHandler

        private void OnIncomingRequestToHighlightField(object sender, FieldHighlightedEventArgs e)
        {
            Guard.AssertNotNull(e, "e");
            Guard.AssertNotEmpty(e.FieldName, "e.FieldName");

            _highlightMessageFilter.Enqueue(e.ArtefactId, e.FieldName);

            var commandArgs = new FieldHighlighted(e.ArtefactId, e.FieldName);

            if (Commands.HighlightField.CanExecute(commandArgs))
                Commands.HighlightField.Execute(commandArgs);
        }

        private void OnPropertyUpdated(object sender, UpdatedPropertyEventArgs e)
        {
            Guard.AssertNotNull(e, "e");
            Guard.AssertNotEmpty(e.PropertyName, "e.PropertyName");

            _propertyChangedMessageFilter.Enqueue(e.ArtefactId, e.PropertyName, null, e.NewValue);
        }

        #endregion EventHandler
    }
}
