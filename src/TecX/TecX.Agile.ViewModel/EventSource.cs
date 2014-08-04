using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public static class EventSource
    {
        #region Events

        public static event EventHandler<HighlightEventArgs> HighlightField = delegate { };

        public static event EventHandler<HighlightEventArgs> FieldHighlighted = delegate { };

        public static event EventHandler<ArtefactPropertyChangedEventArgs> PropertyChanged = delegate { };

        #endregion Events

        #region Methods

        public static void RaiseFieldHighlighted(object sender, HighlightEventArgs args)
        {
            Guard.AssertNotNull(sender, "sender");
            Guard.AssertNotNull(args, "args");

            FieldHighlighted(sender, args);
        }

        public static void RaiseHighlightField(object sender, HighlightEventArgs args)
        {
            Guard.AssertNotNull(sender, "sender");
            Guard.AssertNotNull(args, "args");

            HighlightField(sender, args);
        }

        public static void RaisePropertyChangedEvent(object sender, ArtefactPropertyChangedEventArgs args)
        {
            Guard.AssertNotNull(sender, "sender");
            Guard.AssertNotNull(args, "args");

            PropertyChanged(sender, args);
        }

        #endregion Methods
    }
}