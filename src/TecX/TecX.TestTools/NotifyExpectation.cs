namespace TecX.TestTools
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;

    using TecX.Common;

    public class NotifyExpectation<T>
    {
        private readonly T owner;
        private readonly Action<T> action;

        public NotifyExpectation(T owner, Action<T> action)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(action, "action");

            this.owner = owner;
            this.action = action;
        }

        public EventExpectation<T, TEventArgs> ShouldNotifyVia<TEventArgs>(string eventName)
            where TEventArgs : EventArgs
        {
            Guard.AssertNotEmpty(eventName, "eventName");

            IObservable<EventPattern<TEventArgs>> observable = Observable.FromEventPattern<TEventArgs>(this.owner, eventName);

            return new EventExpectation<T, TEventArgs>(this.owner, this.action, observable);
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

            return new EventExpectation<T, TEventArgs>(this.owner, this.action, observable);
        }
    }
}