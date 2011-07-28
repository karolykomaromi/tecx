using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public static class HighlightEventHub
    {
        #region Events

        public static event EventHandler<HighlightEventArgs> HighlightFieldRequested = delegate { };

        public static event EventHandler<HighlightEventArgs> FieldHighlighted = delegate { };

        #endregion Events

        #region Methods

        public static void RaiseFieldHighlighted(object sender, HighlightEventArgs args)
        {
            Guard.AssertNotNull(sender, "sender");
            Guard.AssertNotNull(args, "args");

            FieldHighlighted(sender, args);
        }

        public static void RaiseHighlightFieldRequested(object sender, HighlightEventArgs args)
        {
            Guard.AssertNotNull(sender, "sender");
            Guard.AssertNotNull(args, "args");

            HighlightFieldRequested(sender, args);
        }

        #endregion Methods
    }
}