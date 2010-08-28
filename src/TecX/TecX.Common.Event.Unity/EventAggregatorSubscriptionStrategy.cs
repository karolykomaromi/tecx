using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Common.Event.Unity
{
    public class EventAggregatorSubscriptionStrategy : BuilderStrategy
    {
        private readonly Dictionary<Type, bool> _knownSubscribers;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregatorSubscriptionStrategy"/> class
        /// </summary>
        public EventAggregatorSubscriptionStrategy()
        {
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
                        GetEventAggregator(context).Subscribe(context.Existing);
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
                        GetEventAggregator(context).Subscribe(context.Existing);
                    }
                    else
                    {
                        //doesnt implement a subscription handler -> dont bother next time
                        _knownSubscribers[type] = false;
                    }
                }
            }
        }

        private static IEventAggregator GetEventAggregator(IBuilderContext context)
        {
            IEventAggregator eventAggregator = context.NewBuildUp<IEventAggregator>();

            if (eventAggregator == null)
            {
                throw new InvalidOperationException("No event aggregator available");
            }
            return eventAggregator;
        }
    }
}