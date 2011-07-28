using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using TecX.Common;

namespace TecX.TestTools
{
    public class NotifyExpectation<T>
    {
        #region Fields

        private readonly T _owner;
        private readonly Action<T> _action;

        #endregion Fields

        #region c'tor

        public NotifyExpectation(T owner, Action<T> action)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(action, "action");

            _owner = owner;
            _action = action;
        }

        #endregion c'tor

        public EventExpectation<T, TEventArgs> ShouldNotifyVia<TEventArgs>(string eventName)
            where TEventArgs : EventArgs
        {
            Guard.AssertNotEmpty(eventName, "eventName");

            IObservable<EventPattern<TEventArgs>> observable = Observable.FromEventPattern<TEventArgs>(_owner, eventName);

            return new EventExpectation<T, TEventArgs>(_owner, _action, observable);
        }

        public EventExpectation<T, TEventArgs> ShouldNotifyVia<TDelegate, TEventArgs>(
            Func<EventHandler<TEventArgs>, TDelegate> conversion,
            Action<TDelegate> addHandler,
            Action<TDelegate> removeHandler)
            where TEventArgs : EventArgs
        {
            Guard.AssertNotNull(conversion, "conversion");
            Guard.AssertNotNull(addHandler, "addHandler");
            Guard.AssertNotNull(removeHandler, "removeHandler");

            IObservable<EventPattern<TEventArgs>> observable = Observable.FromEventPattern(conversion, addHandler, removeHandler);

            return new EventExpectation<T, TEventArgs>(_owner, _action, observable);
        }
    }
}