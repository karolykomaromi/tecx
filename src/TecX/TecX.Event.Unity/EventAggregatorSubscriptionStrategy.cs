namespace TecX.Event.Unity
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;
    using TecX.Event;

    public class EventAggregatorSubscriptionStrategy : BuilderStrategy
    {
        private readonly IEventAggregator eventAggregator;

        private readonly SubscriberTypesCollection knownSubscribers;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregatorSubscriptionStrategy"/> class
        /// </summary>
        /// <param name="eventAggregator">The <see cref="IEventAggregator"/> instance used for this application</param>
        public EventAggregatorSubscriptionStrategy(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.eventAggregator = eventAggregator;

            this.knownSubscribers = new SubscriberTypesCollection();
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            if (context.Existing != null)
            {
                Type type = context.Existing.GetType();

                // if we came across that type before see wether we should automatically
                // subscribe it
                bool shouldSubscribe = this.knownSubscribers.IsSubscriberType(type);

                if (shouldSubscribe)
                {
                    this.eventAggregator.Subscribe(context.Existing);
                }
            }
        }
    }
}