using System;

using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.Infrastructure
{
    public class EventAggregatorAccessor
    {
        private static IEventAggregator _eventAggregator;

        public static IEventAggregator EventAggregator
        {
            get
            {
                if (_eventAggregator == null)
                {
                    throw new InvalidOperationException("The property EventAggregator was not initialized. You must either set this static property" +
                        " directly or use the constructor that takes an IEventAggregator as a parameter and and wire it up in a DI container using c'tor" +
                        " injection prior to accessing the property.");
                }

                return _eventAggregator;
            }
            set
            {
                Guard.AssertNotNull(value, "value");

                _eventAggregator = value;
            }
        }

        public EventAggregatorAccessor(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;
        }
    }
}
