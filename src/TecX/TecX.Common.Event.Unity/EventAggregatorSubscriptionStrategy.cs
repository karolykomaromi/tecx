using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Common.Event.Unity
{
    public class EventAggregatorSubscriptionStrategy : BuilderStrategy
    {
        private readonly Dictionary<Type, bool> _knownSubscribers;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregatorSubscriptionStrategy"/> class
        /// </summary>
        /// <param name="eventAggregator">The <see cref="IEventAggregator"/> instance used for this application</param>
        public EventAggregatorSubscriptionStrategy(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;
            _knownSubscribers = new Dictionary<Type, bool>();
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            if (context.Existing != null)
            {
                Type type = context.Existing.GetType();

                //if we came across that type before see wether we should automatically
                //subscribe it
                bool shouldSubscribe;
                if (_knownSubscribers.TryGetValue(type, out shouldSubscribe))
                {
                    if (shouldSubscribe)
                    {
                        _eventAggregator.Subscribe(context.Existing);
                    }
                }
                else
                {
                    //havent seen that type before so check wether it implements
                    //any subscription handler
                    if (type.GetInterfaces()
                        .Any(i => i.IsGenericType &&
                                  (i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>))))
                    {
                        //if it does add the type to knownSubscribers so we dont have
                        //to check again next time
                        _knownSubscribers[type] = true;
                        _eventAggregator.Subscribe(context.Existing);
                    }
                    else
                    {
                        //doesnt implement a subscription handler -> dont bother next time
                        _knownSubscribers[type] = false;
                    }
                }
            }
        }
    }
}