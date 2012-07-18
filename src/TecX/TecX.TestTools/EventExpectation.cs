namespace TecX.TestTools
{
    using System;
    using System.Reactive;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common;

    public class EventExpectation<T, TEventArgs>
        where TEventArgs : EventArgs
    {
        private readonly ManualResetEvent manualResetEvent;
        private readonly Action<T> action;
        private readonly T owner;
        private readonly IObservable<EventPattern<TEventArgs>> observable;
        private Action<TEventArgs> verify;
        private bool eventRaised;

        public EventExpectation(T owner, Action<T> action, IObservable<EventPattern<TEventArgs>> observable)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(observable, "observable");
            Guard.AssertNotNull(owner, "owner");

            this.action = action;
            this.owner = owner;
            this.observable = observable;

            this.eventRaised = false;
            this.manualResetEvent = new ManualResetEvent(false);
        }

        public EventExpectation<T, TEventArgs> WithArgs(Action<TEventArgs> verify)
        {
            Guard.AssertNotNull(verify, "verify");

            this.verify = verify;

            return this;
        }

        public void AssertExpectations()
        {
            this.eventRaised = false;

            using (this.observable.Subscribe(e =>
                                             {
                                                 try
                                                 {
                                                     if (this.verify != null)
                                                     {
                                                         this.verify(e.EventArgs);
                                                     }

                                                     this.eventRaised = true;
                                                 }
                                                 finally
                                                 {
                                                     this.manualResetEvent.Set();
                                                 }
                                             }))
            {
                this.action(this.owner);
                this.manualResetEvent.WaitOne(1500, false);
            }

            Assert.IsTrue(this.eventRaised);
        }
    }
}