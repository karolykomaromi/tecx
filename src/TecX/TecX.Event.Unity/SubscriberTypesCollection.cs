namespace TecX.Event.Unity
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    public class SubscriberTypesCollection
    {
        private readonly Dictionary<Type, bool> knownSubscribers;

        public SubscriberTypesCollection()
        {
            this.knownSubscribers = new Dictionary<Type, bool>();
        }

        public bool IsSubscriberType(Type type)
        {
            Guard.AssertNotNull(type, "type");

            bool shouldSubscribe;
            if (this.knownSubscribers.TryGetValue(type, out shouldSubscribe))
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
                    this.knownSubscribers[type] = true;
                    return true;
                }
            }

            // doesn't implement a subscription handler -> dont bother next time
            this.knownSubscribers[type] = false;
            return false;
        }
    }
}