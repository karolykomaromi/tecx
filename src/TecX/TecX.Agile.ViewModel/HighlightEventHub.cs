using System;

using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel
{
    public class HighlightEventHub
    {
        private static IEventAggregator _eventAggregator;

        public static IEventAggregator EventAggregator
        {
            get
            {
                if (_eventAggregator == null)
                    throw new InvalidOperationException("EventAggregator was not initialized. Either inject an " +
                        "EventAggregator using the constructor of this class (e.g. use a DI container and ctor injection) " +
                        "or directly set the static property before trying to get a value out of it");

                return _eventAggregator;
            }
            set
            {
                Guard.AssertNotNull(value, "value");

                _eventAggregator = value;
            }
        }

        public HighlightEventHub(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            EventAggregator = eventAggregator;
        }


        #region Events

        public static event EventHandler<HighlightEventArgs> HighlightField = delegate { };

        public static event EventHandler<HighlightEventArgs> FieldHighlighted = delegate { };

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

        #endregion Methods
    }
}