using System;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;
using TecX.Common;

namespace TecX.Agile.Remote
{
    public class WcfPeerMessageRelay : IMessageRelay
    {
        #region Fields

        private readonly IPeerClient _peerClient;
        private readonly HighlightMessageFilter _highlightMessageFilter;
        private readonly PropertyChangedMessageFilter _propertyChangedMessageFilter;
        private readonly StoryCardMovedMessageFilter _storyCardMovedMessageFilter;

        #endregion Fields

        #region c'tor

        public WcfPeerMessageRelay(IPeerClient peerClient)
        {
            Guard.AssertNotNull(peerClient, "peerClient");

            _peerClient = peerClient;

            _peerClient.IncomingRequestToHighlightField += OnIncomingRequestToHighlightField;
            _peerClient.PropertyUpdated += OnPropertyUpdated;
            _peerClient.StoryCardMoved += OnStoryCardMoved;
            _peerClient.CaretMoved += OnCaretMoved;

            _highlightMessageFilter = new HighlightMessageFilter();
            _propertyChangedMessageFilter = new PropertyChangedMessageFilter();
            _storyCardMovedMessageFilter = new StoryCardMovedMessageFilter();
        }

        #endregion c'tor

        #region EventAggregator Subscriptions

        public void Handle(PropertyUpdated message)
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
            Guard.AssertNotNull(message.StoryCardId, "message.StoryCard");

            throw new NotImplementedException();
        }

        public void Handle(StoryCardPostponed message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotNull(message.From, "message.From");
            Guard.AssertNotNull(message.StoryCardId, "message.StoryCard");

            throw new NotImplementedException();
        }

        public void Handle(FieldHighlighted message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.FieldName, "message.FieldName");

            if (_highlightMessageFilter.ShouldLetPass(message))
            {
                _peerClient.Highlight(_peerClient.Id, message.ArtefactId, message.FieldName);
            }
        }

        public void Handle(StoryCardMoved message)
        {
            Guard.AssertNotNull(message, "message");

            if (_storyCardMovedMessageFilter.ShouldLetPass(message))
            {
                _peerClient.MoveStoryCard(_peerClient.Id, message.StoryCardId, message.From, message.To);
            }
        }

        public void Handle(CaretMoved message)
        {
            Guard.AssertNotNull(message, "message");

            _peerClient.MoveCaret(_peerClient.Id, message.ArtefactId, message.FieldName, message.CaretIndex);
        }

        #endregion EventAggregator Subscriptions

        #region EventHandler

        private void OnIncomingRequestToHighlightField(object sender, FieldHighlightedEventArgs e)
        {
            Guard.AssertNotNull(e, "e");
            Guard.AssertNotEmpty(e.FieldName, "e.FieldName");

            var @event = new FieldHighlighted(e.ArtefactId, e.FieldName);

            _highlightMessageFilter.Enqueue(@event);

            if (Commands.HighlightField.CanExecute(@event))
                Commands.HighlightField.Execute(@event);
        }
        private void OnPropertyUpdated(object sender, UpdatedPropertyEventArgs e)
        {
            Guard.AssertNotNull(e, "e");
            Guard.AssertNotEmpty(e.PropertyName, "e.PropertyName");

            var @event = new PropertyUpdated(e.ArtefactId, e.PropertyName, e.OldValue, e.NewValue);

            _propertyChangedMessageFilter.Enqueue(@event);

            if (Commands.UpdateProperty.CanExecute(@event))
                Commands.UpdateProperty.Execute(@event);
        }

        private void OnStoryCardMoved(object sender, StoryCardMovedEventArgs e)
        {
            Guard.AssertNotNull(e, "e");

            StoryCardMoved @event = new StoryCardMoved(e.StoryCardId, e.From, e.To);

            _storyCardMovedMessageFilter.Enqueue(@event);

            if (Commands.MoveStoryCard.CanExecute(@event))
                Commands.MoveStoryCard.Execute(@event);
        }

        private void OnCaretMoved(object sender, CaretMovedEventArgs e)
        {
            Guard.AssertNotNull(e, "e");

            var commandArgs = new CaretMoved(e.ArtefactId, e.FieldName, e.CaretIndex);

            if (Commands.MoveCaret.CanExecute(commandArgs))
                Commands.MoveCaret.Execute(commandArgs);
        }

        #endregion EventHandler
    }
}
