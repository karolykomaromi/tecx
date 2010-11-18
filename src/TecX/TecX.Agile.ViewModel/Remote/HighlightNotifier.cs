using TecX.Agile.ViewModel.Messages;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel.Remote
{
    public class HighlightNotifier : 
        ISubscribeTo<IncomingRequestToHighlightField>
    {
        private readonly IEventAggregator _eventAggregator;

        public HighlightNotifier(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;

            RemoteHighlight.OutgoingNotificationThatFieldWasHighlighted += OnOutgoingNotificationThatFieldWasHighlighted;
        }

        private void OnOutgoingNotificationThatFieldWasHighlighted(object sender, RemoteHighlightEventArgs e)
        {
            Guard.AssertNotNull(e, "e");
            Guard.AssertNotEmpty(e.FieldName, "e.FieldName");

            _eventAggregator.Publish(new OutgoingNotificationToHighlightField(e.ArtefactId, e.FieldName));
        }

        #region Implementation of ISubscribeTo<in IncomingRequestToHighlightField>

        public void Handle(IncomingRequestToHighlightField message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.FieldName, "message.FieldName");

            RemoteHighlight.RaiseIncomingRequestToHighlightField(this, new RemoteHighlightEventArgs(message.ArtefactId, message.FieldName));
        }

        #endregion
    }
}
