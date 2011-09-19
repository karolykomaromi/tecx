using System;

using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Common.Event.Unity
{
    public class EventAggregatorSubscriptionStrategy : BuilderStrategy
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly SubscriberTypesCollection _knownSubscribers;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregatorSubscriptionStrategy"/> class
        /// </summary>
        /// <param name="eventAggregator">The <see cref="IEventAggregator"/> instance used for this application</param>
        public EventAggregatorSubscriptionStrategy(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;

            _knownSubscribers = new SubscriberTypesCollection();
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            if (context.Existing != null)
            {
                Type type = context.Existing.GetType();

                // if we came across that type before see wether we should automatically
                // subscribe it
                bool shouldSubscribe = _knownSubscribers.IsSubscriberType(type);

                if (shouldSubscribe)
                {
                    _eventAggregator.Subscribe(context.Existing);
                }
            }
        }
    }
}