using System;
using System.Collections.Generic;

namespace TecX.Common.Event.Unity
{
    public class SubscriberTypesCollection
    {
        private readonly Dictionary<Type, bool> _knownSubscribers;

        public SubscriberTypesCollection()
        {
            _knownSubscribers = new Dictionary<Type, bool>();
        }

        public bool IsSubscriberType(Type type)
        {
            Guard.AssertNotNull(type, "type");

            bool shouldSubscribe;
            if (_knownSubscribers.TryGetValue(type, out shouldSubscribe))
            {
                return shouldSubscribe;
            }

            // haven't seen that type before so check wether it implements
            // any subscription handler
            foreach (Type i in type.GetInterfaces())
            {
                if (i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)))
                {
                    // if it does add the type to knownSubscribers so we dont have
                    // to check again next time
                    _knownSubscribers[type] = true;
                    return true;
                }
            }

            // doesn't implement a subscription handler -> dont bother next time
            _knownSubscribers[type] = false;
            return false;
        }
    }
}