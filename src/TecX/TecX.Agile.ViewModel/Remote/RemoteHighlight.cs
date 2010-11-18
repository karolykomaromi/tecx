using System;

using TecX.Common;

namespace TecX.Agile.ViewModel.Remote
{
    public static class RemoteHighlight
    {
        #region Events

        public static event EventHandler<RemoteHighlightEventArgs> IncomingRequestToHighlightField = delegate { };

        public static event EventHandler<RemoteHighlightEventArgs> OutgoingNotificationThatFieldWasHighlighted = delegate { };

        #endregion Events

        #region Methods

        public static void RaiseOutgoingNotificationThatFieldWasHighlighted(object sender, RemoteHighlightEventArgs args)
        {
            Guard.AssertNotNull(sender, "sender");
            Guard.AssertNotNull(args, "args");

            OutgoingNotificationThatFieldWasHighlighted(sender, args);
        }

        public static void RaiseIncomingRequestToHighlightField(object sender, RemoteHighlightEventArgs args)
        {
            Guard.AssertNotNull(sender, "sender");
            Guard.AssertNotNull(args, "args");

            IncomingRequestToHighlightField(sender, args);
        }

        #endregion Methods
    }
}